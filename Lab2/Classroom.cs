using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    enum ClassroomStatus
    {
        Fine,
        RepairsRequired,
        UnderRepair
    }

    internal abstract class Classroom : IClassroom
    {
        protected Classroom(int number) 
        {
            Number = number;

            int status = new Random().Next(1, 10);
            if (status >= 5) 
            {
                Status = ClassroomStatus.Fine;   
            } else
            {
                Status = ClassroomStatus.RepairsRequired;
            }
        }
        public int Number { get; private set; }
        public ClassroomStatus Status { get; protected set; }
        public abstract void Repair();
    }
}
