using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;

namespace YidanSoft.Tool
{
    public class SendOrderDataToHis
    {
        private void SendOrderDataToHis(OrderTable currentTable, DataTable changedTable, string executorCode, string macAddress)
        {
            //if (CoreBusinessLogic.BusinessLogic.ConnectToHis)
            //{
            // 数据集转换成byte数组
            MemoryStream source = new MemoryStream();
            changedTable.WriteXml(source, XmlWriteMode.WriteSchema);
            source.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[source.Length];
            source.Read(data, 0, (int)source.Length);

            // 3 调用接口检查数据
            CallDoctorAdviceService(ExchangeInfoOrderConst.MsgCheckData, data, executorCode, macAddress, currentTable.IsTempOrder);

            // 4 调用接口同步数据
            CallDoctorAdviceService(ExchangeInfoOrderConst.MsgSaveData, data, executorCode, macAddress, currentTable.IsTempOrder);

            // 同步成功，则更新同步标志
            UpdateSynchFlagToTrue(currentTable, changedTable);

            currentTable.AcceptChanges();
            //}
        }
    }
}
