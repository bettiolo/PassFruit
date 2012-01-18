using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassFruit.Contracts;

namespace PassFruit.Client.FakeRepository {

    public class Info : IRepositoryInfo {
        
        public string Name {
            get { return "Fake Repository"; }
        }

        public string Description {
            get { return "Fake Repository that returns fixed example data"; }
        }

    }

}
