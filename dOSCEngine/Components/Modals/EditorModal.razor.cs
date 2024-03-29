﻿using dOSCEngine.Engine;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Services;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components.Modals
{
    public partial class EditorModal : IDisposable
    {
        [Inject] ServiceBundle? sb { get; set; }
        ModalV2? Modal;
        BaseNode? SelectedBaseNode;

        protected override void OnInitialized()
        {
            if(sb != null)
            {
                sb.OnNodeEdit += OnEdit;
            }
        }
        
        private void OnEdit(BaseNode node)
        {
            SelectedBaseNode = node;
            if(SelectedBaseNode != null && Modal != null)
            {
                Modal.Open();
                InvokeAsync(() => {
                    StateHasChanged();
                }).ConfigureAwait(false) ;
            }
        }


        private void Updated()
        {
            Modal?.Close();
        }




        public void Dispose()
        {
            if(sb != null)
            {
                sb.OnNodeEdit -= OnEdit;
            }
        }
    }
}
