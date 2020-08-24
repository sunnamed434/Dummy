﻿using Cysharp.Threading.Tasks;
using EvolutionPlugins.Dummy.API;
using OpenMod.Core.Commands;
using SDG.Unturned;
using Steamworks;
using System;
using System.Threading.Tasks;
using Command = OpenMod.Core.Commands.Command;

namespace EvolutionPlugins.Dummy.Commands
{
    [Command("stance")]
    [CommandSyntax("<id> <stance>")]
    [CommandParent(typeof(CommandDummy))]
    public class CommandDummyStance : Command
    {
        private readonly IDummyProvider m_DummyProvider;
        public CommandDummyStance(IServiceProvider serviceProvider, IDummyProvider dummyProvider) : base(serviceProvider)
        {
            m_DummyProvider = dummyProvider;
        }

        protected override async Task OnExecuteAsync()
        {
            if (Context.Parameters.Count == 0)
            {
                throw new CommandWrongUsageException(Context);
            }
            var id = await Context.Parameters.GetAsync<ulong>(0);
            var stance = Context.Parameters[1];

            var dummy = await m_DummyProvider.GetPlayerDummy(id);

            if (!Enum.TryParse<EPlayerStance>(stance.ToUpper(), out var eStance))
            {
                await PrintAsync($"Unable to find a stance: {stance}");
                return;
            }
            await UniTask.SwitchToMainThread();
            dummy.Data.UnturnedUser.Player.stance.checkStance(eStance, false);
        }
    }
}
