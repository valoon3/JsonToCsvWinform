using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Core;
using System.Threading;

namespace WinFormsApp1.FileController
{
    public class Vo
    {
        public String SerialNo { get; set; }
        public String ElapsedTime { get; set; }
        public String CycleSeq { get; set; }
        public String StepType { get; set; }
        public String StepSeq { get; set; }
        public String Temp1 { get; set; }

    }

    internal class JsonFileToDotNet
    {
        public String JsonFilePath { get; set; }
        public String JsonFileName { get; set; }
        public String DotNetFilePath { get; set; }
        public String DotNetFileName { get; set; }

        public JsonFileToDotNet(String JsonFilePath, String JsonFileName, String DotNetFilePath, String DotNetFileName)
        {
            // Sample path
            this.JsonFilePath = JsonFilePath;
            this.JsonFileName = JsonFileName;
            // 엑셀파일 저장 path 
            this.DotNetFilePath = DotNetFilePath;
            this.DotNetFileName = DotNetFileName;
        }

        public List<String> fileToList() // 파일을 읽어와서 json 리스트로 변환
        {

            List<String> list = new List<String>();


            try
            {
                using (FileStream fs = File.Open(JsonFilePath + "\\" + JsonFileName, FileMode.Open))
                {
                    char a = (char)fs.ReadByte();

                    String temp = "";
                    while (!a.Equals(']'))
                    {
                        a = (char)fs.ReadByte();
                        if (!a.Equals('}'))
                        {
                            temp += a;
                        }
                        else
                        {
                            temp += a;
                            if (temp[0] == ',')
                            {
                                temp = temp.Substring(1);
                            }

                            list.Add(temp);
                            temp = "";
                        }

                    }
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch
            {
                Thread.Sleep(10);
                using (FileStream fs = File.Open(JsonFilePath + "\\" + JsonFileName, FileMode.Open))
                {
                    char a = (char)fs.ReadByte();

                    String temp = "";
                    while (!a.Equals(']'))
                    {
                        a = (char)fs.ReadByte();
                        if (!a.Equals('}'))
                        {
                            temp += a;
                        }
                        else
                        {
                            temp += a;
                            if (temp[0] == ',')
                            {
                                temp = temp.Substring(1);
                            }

                            list.Add(temp);
                            temp = "";
                        }

                    }
                    fs.Close();
                    fs.Dispose();
                }
            }
            return list;
        }

        public List<Vo> listToVo() // VoList 를 반환한다.
        {
            List<Vo> list = new List<Vo>();
            List<String> fileList = fileToList();

            foreach (String file in fileList)
            {
                JObject json = JObject.Parse(file);
                Vo vo = new Vo();
                vo.SerialNo = json["SerialNo"].ToString();
                vo.ElapsedTime = json["ElapsedTime"].ToString();
                vo.CycleSeq = json["CycleSeq"].ToString();
                vo.StepType = json["StepType"].ToString();
                vo.StepSeq = json["StepSeq"].ToString();
                vo.Temp1 = json["Temp1"].ToString();

                list.Add(vo);

            }

            return list;

        }

        public void fileCreate()
        {
            List<Vo> vo = listToVo();

            FileInfo excelFile = new FileInfo(DotNetFilePath + "\\" + DotNetFileName);
            if (excelFile.Exists)
                excelFile.Delete();

            string[] sheets = new string[] { "result" };

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                // 시트가 하나이므로 추가적인 작업은 없음.
                excel.Workbook.Worksheets.Add(sheets[0]);


                // dataRow로 데이터를 지정해서 저장합니다.


                // #1. 시트 지정
                ExcelWorksheet sheet = excel.Workbook.Worksheets[sheets[0]];

                // #2. 지정한 데이터 저장하기
                // 헤더 저장

                // 각 아이템에 대한 데이터 저장 - 범위에 의해 가로에 순차적으로 저장된다.
                // 제품ID, 이름, 최저가, 최저가 쇼핑몰, URL

                int idx = 1;

                /// *****************************
                /// 출력 데이터 열이 변경될 경우 수정
                int itemCnt = 6;


                // 각 list 객체가 한 열이며, 각 obj는 해당 행에 대한 데이터를 가지고 있다.
                List<Object[]> dataRows = new List<Object[]>();
                Object[] dataRow = new string[] { "SerialNo", "ElapsedTime", "CycleSeq", "StepType", "StepSeq", "Temp1" };

                dataRows.Add(dataRow);

                foreach (Vo v in vo)
                {
                    Object[] temp = new string[itemCnt];
                    temp[0] = v.SerialNo;
                    temp[1] = v.ElapsedTime;
                    temp[2] = v.CycleSeq;
                    temp[3] = v.StepType;
                    temp[4] = v.StepSeq;
                    temp[5] = v.Temp1;

                    dataRows.Add(temp);

                    idx++;
                }

                // 열 번호는 0부터 시작이므로 0으로 고정. 데이터 갯수에 따라 행 번호만 설정한다.
                string headerRange = "A1:" + Char.ConvertFromUtf32(itemCnt + 64) + "1";
                sheet.Cells[headerRange].LoadFromArrays(dataRows);



                // #3. 컬럼 스타일 지정하기
                // 데이터를 추가하면 컬럼은 자동으로 생성되며, 시작 인덱스는 1 입니다.

                // 각 열의 width 를 지정
                sheet.Column(1).Width = 20;
                sheet.Column(2).Width = 40;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;


                // 각 열 별로 서식을 지정
                /*sheet.Column(1).Style.Numberformat.Format = "@";
                sheet.Column(2).Style.Numberformat.Format = "@";
                sheet.Column(3).Style.Numberformat.Format = @"₩#,##0_);(₩#,##0)"; // 원표시, 3단위별 단위표시
                sheet.Column(4).Style.Numberformat.Format = "@";
                sheet.Column(5).Style.Numberformat.Format = "@";*/

                excel.SaveAs(excelFile);
                excel.Dispose();

            }



        }

    }
}
