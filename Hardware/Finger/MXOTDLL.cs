using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Hardware.Finger
{
    public class MXOTDLL
    {   
        [DllImport("MXOTDLL.dll")]//打开设备
        public static extern int MXInitDev(IntPtr hwndPreview);

        [DllImport("MXOTDLL.dll")]//释放设备
        public static extern int MXReleaseDev();

        [DllImport("MXOTDLL.dll")]//探测手指是否按在采集仪上
        public static extern int MXIsFingerOn();

        [DllImport("MXOTDLL.dll")]//获取的数据是否是手指
        public static extern int MXDetectFinger(char[] FingerBuf);

        [DllImport("MXOTDLL.dll")]//获取图像
        public static extern int MXReadFingerFromSensor(Byte[] FingerBuf);

        [DllImport("MXOTDLL.dll")]//保存图像
        public static extern int MXSaveFingerToFile(Byte[] FingerBuf, string strFingerName);

        [DllImport("MXOTDLL.dll")]//从文件中载入图像
        public static extern int MXLoadFingerFromFile(string strFingerName, Byte[] FingerBuf);

        [DllImport("MXOTDLL.dll")]//保存特征
        public static extern int MXSaveFeatureToFile(Byte[] FeatureBuf, string strFeatureName);

        [DllImport("MXOTDLL.dll")]//提取特征
        public static extern int MXExtract(Byte[] FingerBuf, Byte[] FeatureBuf);

        [DllImport("MXOTDLL.dll")]//指纹比对
        public static extern int MXIdentify(Byte[] SrcFeatureBuf, Byte[] DesFeatureBuf);

        [DllImport("MXOTDLL.dll")]//两次合并模板
        public static extern int MXMerge2(Byte[] Feature1, Byte[] Feature2, Byte[] Templet);

    }
}
