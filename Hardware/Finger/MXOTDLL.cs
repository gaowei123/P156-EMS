using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Hardware.Finger
{
    public class MXOTDLL
    {   
        [DllImport("MXOTDLL.dll")]//���豸
        public static extern int MXInitDev(IntPtr hwndPreview);

        [DllImport("MXOTDLL.dll")]//�ͷ��豸
        public static extern int MXReleaseDev();

        [DllImport("MXOTDLL.dll")]//̽����ָ�Ƿ��ڲɼ�����
        public static extern int MXIsFingerOn();

        [DllImport("MXOTDLL.dll")]//��ȡ�������Ƿ�����ָ
        public static extern int MXDetectFinger(char[] FingerBuf);

        [DllImport("MXOTDLL.dll")]//��ȡͼ��
        public static extern int MXReadFingerFromSensor(Byte[] FingerBuf);

        [DllImport("MXOTDLL.dll")]//����ͼ��
        public static extern int MXSaveFingerToFile(Byte[] FingerBuf, string strFingerName);

        [DllImport("MXOTDLL.dll")]//���ļ�������ͼ��
        public static extern int MXLoadFingerFromFile(string strFingerName, Byte[] FingerBuf);

        [DllImport("MXOTDLL.dll")]//��������
        public static extern int MXSaveFeatureToFile(Byte[] FeatureBuf, string strFeatureName);

        [DllImport("MXOTDLL.dll")]//��ȡ����
        public static extern int MXExtract(Byte[] FingerBuf, Byte[] FeatureBuf);

        [DllImport("MXOTDLL.dll")]//ָ�Ʊȶ�
        public static extern int MXIdentify(Byte[] SrcFeatureBuf, Byte[] DesFeatureBuf);

        [DllImport("MXOTDLL.dll")]//���κϲ�ģ��
        public static extern int MXMerge2(Byte[] Feature1, Byte[] Feature2, Byte[] Templet);

    }
}
