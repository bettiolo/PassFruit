using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Contracts {
    
    public interface IAccountLabels {

        IList<IAccountLabel> GetAll();

    }

}
