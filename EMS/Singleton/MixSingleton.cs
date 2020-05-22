using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMS.Singleton
{

    //单利模式, 创建Mix的单利.
    class MixSingleton
    {
        private MixSingleton()
        {

        }

        public static EMS.Transaction.Mix GetInstance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            // 显式静态构造告诉C＃编译器
            // 未标记类型BeforeFieldInit
            static Nested()
            {
            }

            internal static readonly EMS.Transaction.Mix instance = new EMS.Transaction.Mix();
        }
    }
    
}
