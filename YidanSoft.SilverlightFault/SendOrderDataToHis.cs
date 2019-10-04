using System;
using System.Data;
using System.IO;
using System.Collections.ObjectModel;
using System.ServiceModel; 

namespace YidanSoft.SilverlightFault
{
    public class SendOrderDataToHis
    {
        public void SendOrderTableToHis(string syxh,DataTable changedTable, string executorCode, string macAddress)
        {
            //Service1.Service1SoapClient cs = new Service1.Service1SoapClient(new BasicHttpBinding(), new EndpointAddress("http://192.168.2.130:8012/Service1.asmx"));
            Service2.Service1SoapClient cs = new Service2.Service1SoapClient();
            //ServiceHost host = new ServiceHost(
            
            //if (CoreBusinessLogic.BusinessLogic.ConnectToHis)
            //{
            // 数据集转换成byte数组
            MemoryStream source = new MemoryStream();
            changedTable.WriteXml(source, XmlWriteMode.WriteSchema);

            source.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[source.Length];
            source.Read(data, 0, (int)source.Length);

            // 3 调用接口检查数据
            //CallDoctorAdviceService(ExchangeInfoOrderConst.MsgCheckData, data, executorCode, macAddress, currentTable.IsTempOrder);
            bool iscqls = true;  //临时医嘱  false  长期医嘱
            cs.CheckAdvises(macAddress, syxh, data, iscqls, executorCode);
                 

            // 4 调用接口同步数据
            //CallDoctorAdviceService(ExchangeInfoOrderConst.MsgSaveData, data, executorCode, macAddress, currentTable.IsTempOrder);
            cs.SaveAdvises(macAddress, syxh, data, iscqls, executorCode,"");

            // 同步成功，则更新同步标志
            //UpdateSynchFlagToTrue(currentTable, changedTable);

            //currentTable.AcceptChanges();
            //}
        }

 
    }
}
