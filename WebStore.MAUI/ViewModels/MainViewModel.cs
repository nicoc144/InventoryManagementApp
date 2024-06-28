using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.MAUI.ViewModels
{
    internal class MainViewModel
    {
        private string title;
        public string Title
        {
            set
            {
                this.title = value;
            }
            get
            {
                return this.title;
            }
        }
    }
}
