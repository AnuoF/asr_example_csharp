/*********************************************************************************************
 *	
 * 文件名称:    Audio.cs
 *
 * 描    述：   音频播放、转换和记录类
 *  
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-15
 *
 * 备    注:	此类是从 Tracker800 V9 中移植过来的，实现音频的播放、转换和记录。
 *              在原有类的基础上添加了 PCM_ASR 音频格式：16K、16位、单音。
 *              需要调用C++库 Audio.dll
 * 
*********************************************************************************************/

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Asr.Public
{
    /// <summary>
    /// 基本音频格式描述类
    /// </summary>
    public class WaveFormat
    {
        /// <summary>
        /// 音频格式类别ID
        /// </summary>
        public ushort wFormatTag;
        /// <summary>
        /// 通道数
        /// </summary>
        public ushort nChannels;
        /// <summary>
        /// 采样率
        /// </summary>
        public uint nSamplesPerSec;
        /// <summary>
        /// 数据量
        /// </summary>
        public uint nAvgBytesPerSec;
        /// <summary>
        /// 块大小
        /// </summary>
        public ushort nBlockAlign;
        /// <summary>
        /// 采样位数
        /// </summary>
        public ushort wBitsPerSample;
        /// <summary>
        /// 扩展信息字节数
        /// </summary>
        public ushort cbSize;

        /// <summary>
        /// 供扩展音频格式描述类使用，集成者需先调用基类Serialize方法，然后再顺序写入扩展信息。
        /// </summary>
        /// <param name="writer">调用writer写入扩展信息</param>
        public virtual void Serialize(BinaryWriter writer)
        {
            writer.Write(wFormatTag);
            writer.Write(nChannels);
            writer.Write(nSamplesPerSec);
            writer.Write(nAvgBytesPerSec);
            writer.Write(nBlockAlign);
            writer.Write(wBitsPerSample);
            writer.Write(cbSize);
        }

        /// <summary>
        /// 从AUDIOFORMAT定义转换到对应的WaveFormat格式
        /// </summary>
        /// <param name="af">AUDIOFORMAT值</param>
        /// <returns>WaveFormat</returns>
        public static WaveFormat GetFormat(AudioFormat af)
        {
            WaveFormat wfx = default(WaveFormat);

            switch (af)
            {
                case AudioFormat.PCM_MONO:
                    wfx = PCM_MONO;
                    break;

                case AudioFormat.PCM_ASR:
                    wfx = PCM_ASR;
                    break;

                case AudioFormat.GSM610_MONO:
                    wfx = GSM610_MONO;
                    break;

                case AudioFormat.MP3_MONO:
                    wfx = MP3_MONO;
                    break;

                case AudioFormat.GSM610_RMTP:
                    wfx = GSM610_RMTP;
                    break;
            }

            return wfx;
        }

        /// <summary>
        /// PCM格式、16位、22.05K采样率、单音、运算快、音质好、41K/S
        /// </summary>
        public static WaveFormat PCM_MONO
        {
            get
            {
                WaveFormat wfx = new WaveFormat();
                wfx.wFormatTag = 1;
                wfx.nChannels = 1;
                wfx.nSamplesPerSec = 22050;
                wfx.nAvgBytesPerSec = 22050 * 2;
                wfx.nBlockAlign = 2;
                wfx.wBitsPerSample = 16;
                wfx.cbSize = 0;

                return wfx;
            }
        }

        /// <summary>
        /// PCM格式、16位、16K采样率、单音、运算快、音质一般、32K/S
        /// </summary>
        public static WaveFormat PCM_ASR
        {
            get
            {
                WaveFormat wfx = new WaveFormat();
                wfx.wFormatTag = 1;
                wfx.nChannels = 1;
                wfx.nSamplesPerSec = 16000;
                wfx.nAvgBytesPerSec = 16000 * 2;
                wfx.nBlockAlign = 2;    // TODO
                wfx.wBitsPerSample = 16;
                wfx.cbSize = 0;

                return wfx;
            }
        }

        /// <summary>
        /// GSM610格式、16位、11.025K采样率、单音、运算快、音质差、4K/S
        /// </summary>
        public static WaveFormat GSM610_MONO
        {
            get
            {
                GSM610WaveFormat wfx = new GSM610WaveFormat();
                wfx.wFormatTag = 0x31;
                wfx.nChannels = 1;
                wfx.nSamplesPerSec = 22050;
                wfx.nAvgBytesPerSec = 22050 * 65 / 320;
                wfx.nBlockAlign = 65;
                wfx.wBitsPerSample = 0;
                wfx.cbSize = 2;
                wfx.wSamplesPerBlock = 320;

                return wfx;
            }
        }

        /// <summary>
        /// RMTP协议格式、16位、11.025K采样率、单音、运算快、音质差、4K/S
        /// </summary>
        public static WaveFormat GSM610_RMTP
        {
            get
            {
                GSM610WaveFormat wfx = new GSM610WaveFormat();
                wfx.wFormatTag = 0x31;
                wfx.nChannels = 1;
                wfx.nSamplesPerSec = 11025;
                wfx.nAvgBytesPerSec = 11025 * 65 / 320;
                wfx.nBlockAlign = 65;
                wfx.wBitsPerSample = 0;
                wfx.cbSize = 2;
                wfx.wSamplesPerBlock = 320;

                return wfx;
            }
        }

        /// <summary>
        /// MP3格式、56K平均比特率、22.05K采样率、单音、运算慢、音质普通、7K/S
        /// </summary>
        public static WaveFormat MP3_MONO
        {
            get
            {
                MPEGLayer3WaveFormat wfx = new MPEGLayer3WaveFormat();
                wfx.wFormatTag = 0x55;
                wfx.nChannels = 1;
                wfx.nSamplesPerSec = 22050;
                wfx.nAvgBytesPerSec = 6000;
                wfx.nBlockAlign = 1;
                wfx.wBitsPerSample = 0;
                wfx.cbSize = 12;
                wfx.wID = 1;
                wfx.fdwFlags = 2;
                wfx.nBlockSize = 182;
                wfx.nFramesPerBlock = 1;
                wfx.nCodecDelay = 0;

                return wfx;
            }
        }
    }

    /// <summary>
    /// 服务端客户端共有的数据类型特性标识 因为WCF的客户端在调用服务契约时 要完成数据传过来的数据的反序列化
    /// 必须要显式知道其类型 故以此特性进行标识
    /// 使用方法 为类添加特性：[GlobalData] 即可
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false)]
    public class GlobalDataAttribute : Attribute
    {
        // 仅做标识使用，故不需要实现任何成员
    }

    /// <summary>
    /// 音频格式
    /// </summary>
    [GlobalData]
    [Serializable]
    public enum AudioFormat
    {
        /// <summary>
        /// PCM格式、16位、22.05K采样率、单音、运算快、音质好、41K/S
        /// </summary>
        [EnumMember(Value = "PCM")]
        PCM_MONO = 0,

        /// <summary>
        /// （ASR 通用）PCM格式、16位、16K采样率、单音、运算快、音质一般、32K/S
        /// </summary>
        [EnumMember(Value = "PCM_ASR")]
        PCM_ASR,

        /// <summary>
        /// GSM610格式、16位、22.05K采样率、单音、运算快、音质差、4K/S
        /// </summary>
        [EnumMember(Value = "GSM610")]
        GSM610_MONO,

        /// <summary>
        /// MP3格式、56K平均比特率、22.05K采样率、单音、运算慢、音质普通、7K/S
        /// </summary>
        [EnumMember(Value = "MP3")]
        MP3_MONO,

        /// <summary>
        /// RMTP协议格式、16位、11.025K采样率、单音、运算快、音质差、4K/S
        /// </summary>
        [EnumMember(Value = "GSM610_RMTP")]
        GSM610_RMTP,
    }

    /// <summary>
    /// 未定义的音频格式
    /// </summary>
    public class UnkownWaveFormat : WaveFormat
    {
        /// <summary>
        /// 扩展数据
        /// </summary>
        public byte[] pbExtra;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            if (pbExtra != null)
            {
                writer.Write(pbExtra);
            }
        }
    }

    /// <summary>
    /// GSM610音频格式描述类
    /// </summary>
    public class GSM610WaveFormat : WaveFormat
    {
        /// <summary>
        /// 每块GSM610数据对应采样数据大小
        /// </summary>
        public short wSamplesPerBlock;

        /// <summary>
        /// 同基类
        /// </summary>
        /// <param name="writer">同基类</param>
        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(wSamplesPerBlock);
        }
    }

    /// <summary>
    /// MP3音频格式描述类
    /// </summary>
    public class MPEGLayer3WaveFormat : WaveFormat
    {
        /// <summary>
        /// ID信息，详查MSDN
        /// </summary>
        public short wID;
        /// <summary>
        /// MP3码率标准，详查MSDN
        /// </summary>
        public int fdwFlags;
        /// <summary>
        /// MP3块大小，详查MSDN
        /// </summary>
        public short nBlockSize;
        /// <summary>
        /// MP3每块对应的侦数，详查MSDN
        /// </summary>
        public short nFramesPerBlock;
        /// <summary>
        /// 编码延迟，详查MSDN
        /// </summary>
        public short nCodecDelay;

        /// <summary>
        /// 同基类
        /// </summary>
        /// <param name="writer">同基类</param>
        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(wID);
            writer.Write(fdwFlags);
            writer.Write(nBlockSize);
            writer.Write(nFramesPerBlock);
            writer.Write(nCodecDelay);
        }
    }

    /// <summary>
    /// 音频播放类
    /// </summary>
    public class AudioPlayer : IDisposable
    {
        [DllImport("Audio.dll")]
        extern static int OpenPlayer(ref IntPtr hd, byte[] format);

        [DllImport("Audio.dll")]
        extern static int Play(IntPtr hd, byte[] pBuf, int cbSize);

        [DllImport("Audio.dll")]
        extern static int ClosePlayer(IntPtr hd);

        private IntPtr _hd;

        private WaveFormat _wfx;

        AudioPlayer(IntPtr hd, WaveFormat wfx)
        {
            _hd = hd;
            _wfx = wfx;
        }

        /// <summary>
        /// 音频播放类
        /// </summary>
        /// <param name="wfx">音频格式说明类</param>
        /// <returns>失败返回null</returns>
        public static AudioPlayer Create(WaveFormat wfx)
        {
            if (wfx == null)
            {
                return null;
            }

            MemoryStream buffer = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(buffer);
            wfx.Serialize(writer);

            IntPtr ps = IntPtr.Zero;
            if (OpenPlayer(ref ps, buffer.ToArray()) != 0)
            {
                return null;
            }

            return new AudioPlayer(ps, wfx);
        }

        /// <summary>
        /// 创建音频播放对象
        /// </summary>
        /// <param name="wfx">音频格式</param>
        /// <param name="multiplier">播放倍速</param>
        /// <returns></returns>
        public static AudioPlayer Create(WaveFormat wfx, double multiplier)
        {
            if (wfx == null || multiplier < 0)
            {
                return null;
            }

            if (!multiplier.Equals(1.0))
            {
                wfx.nSamplesPerSec = (uint)(wfx.nSamplesPerSec * multiplier);
            }

            return Create(wfx);
        }

        /// <summary>
        /// 播放音频，当数据大于缓冲大小时阻塞到可用缓冲为止，缓冲大小默认1秒。
        /// </summary>
        /// <param name="pBuf">音频数据，数据应对齐块边界,即音频数据长度能整除nBlockAlign值，否则可能出现杂音。</param>
        public void Play(byte[] pBuf)
        {
            if (IntPtr.Zero != _hd)
            {
                if (0 == Play(_hd, pBuf, pBuf.Length))
                {
                    return;
                }
            }

            _hd = IntPtr.Zero;

            MemoryStream buffer = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(buffer);
            _wfx.Serialize(writer);

            if (0 != OpenPlayer(ref _hd, buffer.ToArray()))
            {
                _hd = IntPtr.Zero;
            }
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Dispose()
        {
            ClosePlayer(_hd);
            _hd = IntPtr.Zero;
        }
    }

    /// <summary>
    /// 音频转换类
    /// </summary>
    public class AudioConvert : IDisposable
    {
        private const int BLOCKALIGN = 0x00000004;
        private const int INIT = 0x00000010;
        private const int END = 0x00000020;

        [DllImport("Audio.dll")]
        extern static int OpenConvert(ref IntPtr phd, byte[] pwfSrc, byte[] pwfDst);

        [DllImport("Audio.dll")]
        extern static int GetRecommendedSize(IntPtr hd, int cbInput, ref int pdwOutputBytes);

        [DllImport("Audio.dll")]
        extern static int Convert(IntPtr has, byte[] pSrcData, int dwSrcBytes, byte[] pDstData, ref int pdwDstBytes, int flag);

        [DllImport("Audio.dll")]
        extern static int CloseConvert(IntPtr hd);

        [DllImport("Audio.dll")]
        extern static int ChooseFormat(IntPtr hDlg, byte[] format, int formatsize);

        /// <summary>
        /// 选择格式窗口
        /// </summary>
        /// <param name="hDlg">窗体句柄</param>
        /// <returns>音频格式</returns>
        public static UnkownWaveFormat ChooseFormat(IntPtr hDlg)
        {
            byte[] format = new byte[128];
            if (ChooseFormat(hDlg, format, format.Length) == 0)
            {
                UnkownWaveFormat wfx = new UnkownWaveFormat();
                wfx.wFormatTag = BitConverter.ToUInt16(format, 0);// 1;
                wfx.nChannels = BitConverter.ToUInt16(format, 2);
                wfx.nSamplesPerSec = BitConverter.ToUInt32(format, 4); ;
                wfx.nAvgBytesPerSec = BitConverter.ToUInt32(format, 8);
                wfx.nBlockAlign = BitConverter.ToUInt16(format, 12);
                wfx.wBitsPerSample = BitConverter.ToUInt16(format, 14);
                wfx.cbSize = BitConverter.ToUInt16(format, 16);
                wfx.pbExtra = new byte[wfx.cbSize];
                Array.Copy(format, 18, wfx.pbExtra, 0, wfx.cbSize);

                return wfx;
            }

            return null;
        }

        private IntPtr _hd;
        AudioConvert(IntPtr hd)
        {
            _hd = hd;
        }

        /// <summary>
        /// 创建转换类实例
        /// </summary>
        /// <param name="src">源音频格式</param>
        /// <param name="dst">目标音频格式</param>
        /// <returns>错误或失败返回null，注意：大多不同音频格式之间不能直接转换，可能需要分步进行，如先调整波特率，再调整格式或通道数。</returns>
        public static AudioConvert Create(WaveFormat src, WaveFormat dst)
        {
            MemoryStream srcbuf = new MemoryStream();
            MemoryStream dstbuf = new MemoryStream();
            BinaryWriter srcwrit = new BinaryWriter(srcbuf);
            BinaryWriter dstwrit = new BinaryWriter(dstbuf);
            src.Serialize(srcwrit);
            dst.Serialize(dstwrit);

            IntPtr has = IntPtr.Zero;
            int result = OpenConvert(ref has, srcbuf.ToArray(), dstbuf.ToArray());
            if (result != 0)
            {
                return null;
            }

            return new AudioConvert(has);
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="end">true表示本次转换是最后一次转换</param>
        /// <returns>失败返回null</returns>
        public byte[] Convert(byte[] data, bool end)
        {
            int dstsize = 0;
            if (GetRecommendedSize(_hd, data.Length, ref dstsize) != 0)
            {
                return null;
            }

            Random rand = new Random();
            byte[] dst = new byte[dstsize];
            rand.NextBytes(dst);

            if (Convert(_hd, data, data.Length, dst, ref dstsize, end ? END : BLOCKALIGN) != 0)
            {
                return null;
            }

            if (dst.Length != dstsize)
            {
                Array.Resize<byte>(ref dst, dstsize);
            }

            return dst;
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Dispose()
        {
            CloseConvert(_hd);
        }
    }

    public delegate void ReadyCallBack(IntPtr lpData, int cbSize);
    /// <summary>
    /// 音频记录类
    /// </summary>
    public class AudioRecorder : IDisposable
    {
        //delegate void ReadyCallBack(IntPtr lpData, int cbSize);

        [DllImport("Audio.dll")]
        extern static int OpenRecorder(ref IntPtr phd, byte[] format, [MarshalAs(UnmanagedType.FunctionPtr)]ReadyCallBack callback);//IntPtr pte);// 

        [DllImport("Audio.dll")]
        extern static int Start(IntPtr hd);

        [DllImport("Audio.dll")]
        extern static int Stop(IntPtr hd);

        [DllImport("Audio.dll")]
        extern static int CloseRecorder(IntPtr hd);

        private IntPtr _hd = IntPtr.Zero;
        private BinaryWriter _writer = null;
        private ReadyCallBack _callback = null;

        AudioRecorder() { }

        /// <summary>
        /// 创建音频采集类事例
        /// </summary>
        /// <param name="wfx">采集音频使用的格式类型，注意：某些音频格式不支持采集</param>
        /// <param name="writer">写入的流对象</param>
        /// <returns>失败返回null</returns>
        public static AudioRecorder Create(WaveFormat wfx, BinaryWriter writer)
        {
            if (writer == null)
            {
                return null;
            }

            MemoryStream wfxbs = new MemoryStream();
            wfx.Serialize(new BinaryWriter(wfxbs));

            AudioRecorder recoder = new AudioRecorder();
            recoder._writer = writer;
            recoder._callback = new ReadyCallBack(recoder.CallBackProc);

            int result = OpenRecorder(ref recoder._hd, wfxbs.GetBuffer(), recoder._callback);
            if (result != 0)
            {
                return null;
            }

            return recoder;
        }

        /// <summary>
        /// 创建音频采集类事例
        /// </summary>
        /// <param name="wfx">采集音频使用的格式类型，注意：某些音频格式不支持采集</param>
        /// <param name="callback">采集到数据后的回调函数</param>
        /// <param name="errcode">错误码</param>
        /// <returns></returns>
        public static AudioRecorder Create(WaveFormat wfx, ReadyCallBack callback, ref int errcode)
        {
            MemoryStream wfxbs = new MemoryStream();
            wfx.Serialize(new BinaryWriter(wfxbs));

            AudioRecorder recoder = new AudioRecorder();
            recoder._callback = callback;

            errcode = OpenRecorder(ref recoder._hd, wfxbs.GetBuffer(), recoder._callback);
            if (errcode != 0)
            {
                return null;
            }

            return recoder;
        }

        private void CallBackProc(IntPtr lpData, int cbSize)
        {
            byte[] pBuf = new byte[cbSize];
            Marshal.Copy(lpData, pBuf, 0, cbSize);
            _writer.Write(pBuf, 0, pBuf.Length);
        }

        /// <summary>
        /// 开始采集音频
        /// </summary>
        public void Start() { Start(_hd); }

        /// <summary>
        /// 停止采集音频
        /// </summary>
        public void Stop() { Stop(_hd); }

        /// <summary>
        /// 回收
        /// </summary>
        public void Dispose()
        {
            CloseRecorder(_hd);
        }
    }
}
