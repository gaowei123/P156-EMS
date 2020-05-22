using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class Slot_Position
    {
        public Slot_Position()
        {

        }

        public Slot_Position(DataRow x)
        {
            SLOT_ID = int.Parse(x["SLOT"].ToString());
            SLOT_INDEX = int.Parse(x["SLOT_LEVEL"].ToString());
            POSITION = int.Parse(x["POSITION"].ToString());
        }

        public int SLOT_ID { get; set; }
        public int SLOT_INDEX { get; set; }
        public int POSITION { get; set; }
    }
}
