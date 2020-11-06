using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class ViewModel
    {
        public ObservableCollection<Person> Persons { get; set; }

        public ViewModel()
        {
            Persons = new ObservableCollection<Person>();
            
        }
    }
}
