/*********************************************************************************************
 *	
 * 文件名称:    ShowInfoEventArgs.cs
 * 
 * 描    述：   显示信息事件结构体
 *
 * 作    者:    Anuo.
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System;

namespace AsrService
{
    /// <summary>
    /// 显示信息事件结构体
    /// </summary>
    internal class ShowInfoEventArgs : EventArgs
    {
        public string Msg;
    }
}
