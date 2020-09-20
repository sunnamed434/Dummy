﻿using Cysharp.Threading.Tasks;
using EvolutionPlugins.Dummy.API;
using EvolutionPlugins.Dummy.Extensions.Interaction.Actions;
using EvolutionPlugins.Dummy.Models;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using Command = OpenMod.Core.Commands.Command;

namespace EvolutionPlugins.Dummy.Commands.Actions
{
    [Command("gesture")]
    [CommandDescription("Make a dummy gesture")]
    [CommandParent(typeof(CommandDummy))]
    [CommandSyntax("<id> <gesture>")]
    public class CommandDummyGesture : CommandDummyAction
    {
        public CommandDummyGesture(IServiceProvider serviceProvider, IDummyProvider dummyProvider) : base(serviceProvider, dummyProvider)
        {
        }

        protected override async UniTask ExecuteDummyAsync(PlayerDummy playerDummy)
        {
            var user = (UnturnedUser)Context.Actor;
            Console.WriteLine(user.Player.Player.stance.transform.position.ToString());
            Console.WriteLine(playerDummy.Data.UnturnedUser.Player.Player.stance.transform.position.ToString());
            return;
            var gesture = await Context.Parameters.GetAsync<string>(1);
            if (!Enum.TryParse<EPlayerGesture>(gesture.ToUpper(), out var eGesture))
            {
                await PrintAsync($"Unable find a gesture {gesture}");
                await PrintAsync($"All gestures: {string.Join(",", Enum.GetNames(typeof(EPlayerGesture)))}");
                return;
            }
            playerDummy.Actions.Actions.Enqueue(new GestureAction(eGesture));
        }
    }
}
