using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameJam2018
{
    interface ICapturable
    {
        void SetOwner(int newOwner);
        int GetOwner();
    }
}
