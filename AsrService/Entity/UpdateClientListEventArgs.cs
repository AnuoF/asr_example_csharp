/*********************************************************************************************
 *	
 * 文件名称:    UpdateClientListEventArgs.cs
 * 
 * 描    述：   更新客户端列表事件结构体
 *
 * 作    者:    Anuo.
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System;
using System.Collections.Generic;

namespace AsrService
{
    /// <summary>
    /// 更新客户端列表事件结构体
    /// </summary>
    internal class UpdateClientListEventArgs : EventArgs
    {
        public List<string> ClientList;
    }
}
