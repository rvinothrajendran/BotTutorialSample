using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotDialogSample
{
    public class StateAccessor
    {
        public ConversationState ConvState;

        public IStatePropertyAccessor<DialogState> DlgState;

        public StateAccessor()
        {
            ConvState = new ConversationState(new MemoryStorage());
            DlgState = ConvState.CreateProperty<DialogState>(nameof(DlgState));
        }
    }
}
