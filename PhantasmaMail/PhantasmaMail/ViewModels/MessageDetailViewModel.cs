using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;

namespace PhantasmaMail.ViewModels
{
    public class MessageDetailViewModel : ViewModelBase
    {
        public InboxMessage SelectedMessage { get; set; }


        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is InboxMessage message)
            {
                SelectedMessage = message;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
