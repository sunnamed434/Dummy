﻿using Cysharp.Threading.Tasks;
using EvolutionPlugins.Dummy.API;
using Microsoft.Extensions.Configuration;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using Command = OpenMod.Core.Commands.Command;

namespace EvolutionPlugins.Dummy.Commands
{
    [Command("copy")]
    [CommandDescription("Creates a dummy and copy your skin, hait, beard, etc...")]
    [CommandActor(typeof(UnturnedUser))]
    [CommandParent(typeof(CommandDummy))]
    public class CommandDummyCopy : Command
    {
        private readonly IConfiguration m_Configuration;
        private readonly IDummyProvider m_DummyProvider;

        public CommandDummyCopy(IServiceProvider serviceProvider, IConfiguration configuration,
            IDummyProvider dummyProvider) : base(serviceProvider)
        {
            m_Configuration = configuration;
            m_DummyProvider = dummyProvider;
        }

        protected override async Task OnExecuteAsync()
        {
            var user = (UnturnedUser)Context.Actor;
            var amountDummiesConfig = m_Configuration.GetSection("AmountDummiesInSameTime").Get<byte>();
            if (amountDummiesConfig != 0 && m_DummyProvider.Dummies.Count + 1 > amountDummiesConfig)
            {
                await user.PrintMessageAsync("Dummy can't be created. Amount dummies overflow", Color.Yellow);
                return;
            }

            var id = m_DummyProvider.GetAvailableId();

            await m_DummyProvider.AddDummyAsync(id, new DummyData() { Owners = new List<CSteamID> { user.SteamId } });

            var steamPlayer = user.SteamPlayer;

            Provider.pending.Add(new SteamPending(new SteamPlayerID(id, 0, "dummy", "dummy", "dummy", CSteamID.Nil),
                true, steamPlayer.face, steamPlayer.hair, steamPlayer.beard, steamPlayer.skin, steamPlayer.color,
                UnityEngine.Color.white, steamPlayer.hand, 0UL, 0UL, 0UL, 0UL, 0UL, 0UL, 0UL, Array.Empty<ulong>(),
                EPlayerSkillset.NONE, "english", CSteamID.Nil));

            await UniTask.SwitchToMainThread();
            Provider.accept(new SteamPlayerID(id, 0, "dummy", "dummy", "dummy", CSteamID.Nil), true, false,
                steamPlayer.face, steamPlayer.hair, steamPlayer.beard, steamPlayer.skin, steamPlayer.color,
                UnityEngine.Color.white, steamPlayer.hand, steamPlayer.shirtItem, steamPlayer.pantsItem, steamPlayer.hatItem,
                steamPlayer.backpackItem, steamPlayer.vestItem, steamPlayer.maskItem, steamPlayer.glassesItem,
                steamPlayer.skinItems, steamPlayer.skinTags, steamPlayer.skinDynamicProps, EPlayerSkillset.NONE,
                "english", CSteamID.Nil);

            var dummy = Provider.clients.Last();
            dummy.player.teleportToLocationUnsafe(user.Player.transform.position, user.Player.transform.rotation.eulerAngles.y);
            await UniTask.SwitchToTaskPool();

            await user.PrintMessageAsync($"Dummy ({id.m_SteamID}) has created");
        }
    }
}