﻿using Cysharp.Threading.Tasks;
using Dummy.API;
using Dummy.Users;
using SDG.Unturned;
using System;
using System.Threading.Tasks;

namespace Dummy.Actions.Interaction.Actions
{
    public class FaceAction : IAction
    {
        public FaceAction(byte index)
        {
            // 32 index face is an empty face (very scary)
            if (index > Customization.FACES_FREE + Customization.FACES_PRO)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            Index = index;
        }

        public byte Index { get; }

        public async Task Do(DummyUser dummy)
        {
            await UniTask.SwitchToMainThread();
            dummy.Player.Player.equipment.channel.send("tellSwapFace", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
            {
                Index
            });
        }
    }
}