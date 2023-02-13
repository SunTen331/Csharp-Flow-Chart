using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppFlowChart
{
    public class Process
    {
        public List<string> process;
        public Process(List<string> blocks)
        {
            process = blocks;
        }
    }

    public class Sequence
    {
        public List<Process> sequence;
        public Sequence(List<Process> processes)
        {
            sequence = processes;
        }
    }

    public class FlowChart
    {
        public static string SCPY = "SCPY:";
        public static string SYNTAX = "SYNTAX:";
        public static string NoneKeyWord = "None";

        public static List<Sequence> GetFlowChart => flowChart;

        public static List<Sequence> flowChart = new List<Sequence>
        {
            new Sequence( new List<Process>
            {
                new Process(new List<string> { SCPY, NoneKeyWord, SYNTAX, NoneKeyWord }),
            }),
            new Sequence( new List<Process>
            {
                new Process(new List<string> { SCPY, "*CALC", SYNTAX, ":CALCulate:CLIMits:CLEar:AUTO?" }),
                new Process(new List<string> { @"YUCC Generated Source Files\se_stub.cpp", "W_scpi_calc_clim_cle_auto_q" }),
                new Process(new List<string> { @"Fcode\calculate.cpp\PROTO_QRY_1",
                    "RETURN_HAP_ERROR_IF_LIMIT_TEST_DISABLED",
                    "ChanSuffixCommandHelper helper",
                    "RETURN_HAP_ERROR_INDEX_IF_ERROR",
                    "GetDefMinMaxForQry",
                    "CalculateControl.GetClimitsClearAuto",
                    "RETURN_HAP_ERROR_INDEX_IF_ERROR",
                    "return helper.GetHappening" }),
            }),
            new Sequence( new List<Process>
            {
                new Process(new List<string> { SCPY, "*CALC", SYNTAX, ":CALCulate:CLIMits:CLEar:AUTO 1" }),
                new Process(new List<string> { @"YUCC Generated Source Files\se_stub.cpp", "W_scpi_calc_clim_cle_auto" }),
                new Process(new List<string> { @"Fcode\calculate.cpp\PROTO_CMD_1",
                    "RETURN_HAP_ERROR_IF_LIMIT_TEST_DISABLED",
                    "ChanSuffixCommandHelper helper",
                    "RETURN_HAP_ERROR_INDEX_IF_ERROR",
                    "RETURN_HAP_OK_IF_EXCLUSIVE_ERROR",
                    "CalculateControl.CheckClimitsClearAuto",
                    "CalculateControl.SetClimitsClearAuto",
                    "CalculateControl.SetSourceClearAuto",
                    "RETURN_HAP_ERROR_INDEX_IF_ERROR",
                    "return helper.GetHappening" }),
            }),
            new Sequence( new List<Process>
            {
                new Process(new List<string> { SCPY, "*CALC", SYNTAX, ":CALCulate:CLIMits:CLEar:AUTO:DELay?" }),
                new Process(new List<string> { @"YUCC Generated Source Files\se_stub.cpp", "W_scpi_calc_clim_cle_auto_del_q" }),
                new Process(new List<string> { @"Fcode\calculate.cpp\PROTO_QRY_1_1",
                    "RETURN_HAP_ERROR_IF_LIMIT_TEST_DISABLED",
                    "ChanSuffixCommandHelper helper",
                    "RETURN_HAP_ERROR_INDEX_IF_ERROR",
                    "GetDefMinMaxForQry",
                    "RETURN_HAP_ERROR_INDEX_IF_ERROR",
                    "return helper.GetHappening" }),
            }),
            new Sequence( new List<Process>
            {
                new Process(new List<string> { SCPY, "*FETC", SYNTAX, ":FETCh:ARRay?" }),
                new Process(new List<string> { @"YUCC Generated Source Files\se_stub.cpp", "W_scpi_fetc_arr_q" }),
                new Process(new List<string> { @"Fcode\fetch.cpp\PROTO_QRY_1_1",
                    "FormatElementSenseToMeasureBlockDataElement",
                    "InitializeMeasureBlockDataInfoForArray",
                    "RETURN_HAP_ERROR_IF_SCPI_ERROR",
                    "WaitAcquireIdle\nFetchSpecifiedArray",
                    "RETURN_HAP_ERROR_INDEX_IF_ERROR",
                    "return helper.GetHappening" }),
            }),
            new Sequence( new List<Process>
            {
                new Process(new List<string> { SCPY, "*ESE", SYNTAX, "*ESE enable_number" }),
                new Process(new List<string> { "A", "c", "cc", "ccc" }),
                new Process(new List<string> { "D", "d", "dd", "dddd" }),
            }),
        };
    }
}
