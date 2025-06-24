using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using MassTransit;

namespace Application.External_Services.MassTransit
{
    public class SubastaStateMachine : MassTransitStateMachine<SubastaState>
    {
        public State Pending { get; set; }
        public State Active { get;  set; }
        public State Ended { get;set; }
        public Event<SubastaIniciadaEvent> SubastaIniciada { get; set; }
        public Event<SubastaFinalizadaEvent> SubastaFinalizada { get; set; }

        public SubastaStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => SubastaIniciada, x => x.CorrelateById(context => context.Message.SubastaId));
            Event(() => SubastaFinalizada, x => x.CorrelateById(context => context.Message.SubastaId));

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
                        await ctx.Publish(new SubastaActivaEvent(ctx.Saga.SubastaId));
                    })
            );

            During(Active,
                When(SubastaFinalizada)
                    .Then(ctx =>
                    {
                        ctx.Saga.FechaFin = DateTime.UtcNow;
                    })
                    .TransitionTo(Ended)
                    .ThenAsync(async ctx =>
                        {
                            await ctx.Publish(new SubastaAcabadaEvent(ctx.Saga.SubastaId));
                        })
            );

            SetCompletedWhenFinalized();
        }
    }

}
