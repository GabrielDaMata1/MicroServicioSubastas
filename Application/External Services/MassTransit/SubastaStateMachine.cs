using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using MassTransit;

namespace Application.External_Services.MassTransit
{
    /// <summary>
    /// Clase de MassTransit que gestiona los estados de una subasta mediante una máquina de estados, que responde a distintos eventos dependiendo del estado actual de la subasta.
    /// </summary>

    public class SubastaStateMachine : MassTransitStateMachine<SubastaState>
    {
        /// <summary>
        /// Atributo que corresponde al estado inicial de la subasta.
        /// </summary>
        public State Pending { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado de la subasta cuando se encuentra activa tras empezar la subasta.
        /// </summary>
        public State Active { get;  set; }
        /// <summary>
        /// Atributo que corresponde al estado de la subasta cuando se encuentra terminada tras terminar la subasta.
        /// </summary>
        public State Ended { get;set; }
        /// <summary>
        /// Atributo que corresponde al estado de la subasta cuando se encuentra completada tras realizar el pago de la misma.
        /// </summary>
        public State Completed { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado de la subasta cuando se encuentra cancelada tras no haber recibido el pago de la subasta
        /// después de cinco días de finalización.
        /// </summary>
        public State Canceled { get; set; }
        /// <summary>
        /// Atributo que indica que la subasta ha sido iniciada tras recibir el evento SubastaIniciadaEvent.
        /// </summary>
        public Event<SubastaIniciadaEvent> SubastaIniciada { get; set; }
        /// <summary>
        /// Atributo que indica que la subasta ha sido finalizada tras recibir el evento SubastaFinalizadaEvent.
        /// </summary>
        public Event<SubastaFinalizadaEvent> SubastaFinalizada { get; set; }
        /// <summary>
        /// Atributo que indica que la subasta ha sido cancelada tras recibir el evento TemporizadorCancelarPagoEvent.
        /// </summary>
        public Event<TemporizadorCancelarPagoEvent> SubastaCancelada { get; set; }
        /// <summary>
        /// Atributo que indica que la subasta ha sido completada tras recibir el evento PagoRealizadoEvent.
        /// </summary>
        public Event<PagoRealizadoEvent> SubastaCompletada { get; set; }

        /// <summary>
        /// Constructor que configura la máquina de estados, definiendo las transiciones y acciones asociadas a cada evento.
        /// </summary>
        public SubastaStateMachine()
        {
            InstanceState(x => x.CurrentState);

            // Se configura la correlación de eventos por el ID de la subasta
            Event(() => SubastaIniciada, x => x.CorrelateById(context => context.Message.SubastaId));
            Event(() => SubastaFinalizada, x => x.CorrelateById(context => context.Message.SubastaId));
            Event(() => SubastaCompletada, x => x.CorrelateById(context => context.Message.idSubasta));
            Event(() => SubastaCancelada, x => x.CorrelateById(context => context.Message.SubastaId));

            //Se transiciona del estado Pending a Active cuando inicie la subasta
            Initially(
                When(SubastaIniciada)
                    .Then(ctx =>
                    {
                        ctx.Saga.SubastaId = ctx.Message.SubastaId;
                        ctx.Saga.FechaInicio = ctx.Message.fechaInicio;
                        ctx.Saga.FechaFin = ctx.Message.fechaFin;
                    })
                    .TransitionTo(Active) 
                    .ThenAsync(async ctx => 
                    {
                        //Se publica el evento de SubastaActivaEvent para que lo consuma un consumidor
                        await ctx.Publish(new SubastaActivaEvent(ctx.Saga.SubastaId));
                    })
            );
            //Se transiciona del estado Active a Ended cuando finalice la subasta
            During(Active,
                When(SubastaFinalizada)
                    .Then(ctx =>
                    {
                        ctx.Saga.FechaFin = DateTime.UtcNow;
                    })
                    .TransitionTo(Ended)
                    .ThenAsync(async ctx =>
                        {
                            //Se publica el evento de SubastaAcabadaEvent para que lo consuma un consumidor
                            await ctx.Publish(new SubastaAcabadaEvent(ctx.Saga.SubastaId));
                        })
            );

            //Se transiciona del estado Ended a Completed cuando se realice el pago de la subasta
            During(Ended,
            When(SubastaCompletada)
                .TransitionTo(Completed)
                .ThenAsync(async ctx =>
                {
                    //Se publica el evento de SubastaCompletadaEvent para que lo consuma un consumidor
                    await ctx.Publish(new SubastaCompletadaEvent(ctx.Saga.SubastaId));
                })
             );

            //Se transiciona del estado Ended a Canceled cuando no se reciba el pago de la subasta
            During(Ended,
            When(SubastaCancelada)
                .TransitionTo(Canceled)
                .ThenAsync(async ctx =>
                {
                    //Se publica el evento de SubastaCanceladaEvent para que lo consuma un consumidor
                    await ctx.Publish(new SubastaCanceladaEvent(ctx.Saga.SubastaId));
                })
             );




            SetCompletedWhenFinalized();
        }
    }

}
