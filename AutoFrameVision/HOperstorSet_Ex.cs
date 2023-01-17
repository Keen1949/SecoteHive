using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using OperationIni;
using System.IO;
using System.Windows.Forms;
using CommonTool;

namespace AutoFrameVision
{
    public static class HOperatorSet_Ex
    {
        public static void spoke(HObject ho_Image, out HObject ho_Regions, HTuple hv_Elements, HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold, HTuple hv_Transition, HTuple hv_Select, HTuple hv_ROIRows, HTuple hv_ROICols, HTuple hv_Direct, out HTuple hv_ResultRow, out HTuple hv_ResultColumn, out HTuple hv_ArcType)
        {
            HObject[] OTemp = new HObject[20];
            HObject ho_Rectangle = null;
            HObject ho_Arrow = null;
            HTuple hv_Width = null;
            HTuple hv_Height = null;
            HTuple hv_RowC = null;
            HTuple hv_ColumnC = null;
            HTuple hv_Radius = null;
            HTuple hv_StartPhi = null;
            HTuple hv_EndPhi = null;
            HTuple hv_PointOrder = null;
            HTuple hv_RowXLD = null;
            HTuple hv_ColXLD = null;
            HTuple hv_Length = null;
            HTuple hv_Length2 = null;
            HTuple hv_j = new HTuple();
            HTuple hv_RowE = new HTuple();
            HTuple hv_ColE = new HTuple();
            HTuple hv_ATan = new HTuple();
            HTuple hv_RowL2 = new HTuple();
            HTuple hv_RowL3 = new HTuple();
            HTuple hv_ColL2 = new HTuple();
            HTuple hv_ColL3 = new HTuple();
            HTuple hv_MsrHandle_Measure = new HTuple();
            HTuple hv_RowEdge = new HTuple();
            HTuple hv_ColEdge = new HTuple();
            HTuple hv_Amplitude = new HTuple();
            HTuple hv_Distance = new HTuple();
            HTuple hv_tRow = new HTuple();
            HTuple hv_tCol = new HTuple();
            HTuple hv_t = new HTuple();
            HTuple hv_Number = new HTuple();
            HTuple hv_k = new HTuple();
            HTuple hv_Select_COPY_INP_TMP = hv_Select.Clone();
            HTuple hv_Transition_COPY_INP_TMP = hv_Transition.Clone();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HObject ho_Contour;
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HObject ho_ContCircle;
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            hv_ArcType = new HTuple();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            ho_Regions.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();
            ho_Contour.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_ROIRows, hv_ROICols);
            HOperatorSet.FitCircleContourXld(ho_Contour, "algebraic", -1, 0, 0, 3, 2, out hv_RowC, out hv_ColumnC, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
            ho_ContCircle.Dispose();
            HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_RowC, hv_ColumnC, hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder, 3);
            HOperatorSet.GetContourXld(ho_ContCircle, out hv_RowXLD, out hv_ColXLD);
            HOperatorSet.LengthXld(ho_ContCircle, out hv_Length);
            HOperatorSet.TupleLength(hv_ColXLD, out hv_Length2);
            if (new HTuple(hv_Elements.TupleLess(1)) != 0)
            {
                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow.Dispose();
            }
            else
            {
                HTuple end_val19 = hv_Elements - 1;
                HTuple step_val19 = 1;
                HTuple hv_i = 0;
                while (hv_i.Continue(end_val19, step_val19))
                {
                    if (new HTuple(hv_RowXLD.TupleSelect(0).TupleEqual(hv_RowXLD.TupleSelect(hv_Length2 - 1))) != 0)
                    {
                        HOperatorSet.TupleInt(1.0 * hv_Length2 / (hv_Elements - 1) * hv_i, out hv_j);
                        hv_ArcType = "circle";
                    }
                    else
                    {
                        HOperatorSet.TupleInt(1.0 * hv_Length2 / (hv_Elements - 1) * hv_i, out hv_j);
                        hv_ArcType = "arc";
                    }
                    if (new HTuple(hv_j.TupleGreaterEqual(hv_Length2)) != 0)
                    {
                        hv_j = hv_Length2 - 1;
                    }
                    hv_RowE = hv_RowXLD.TupleSelect(hv_j);
                    hv_ColE = hv_ColXLD.TupleSelect(hv_j);
                    if (new HTuple(new HTuple(new HTuple(hv_RowE.TupleGreater(hv_Height - 1)).TupleOr(new HTuple(hv_RowE.TupleLess(0)))).TupleOr(new HTuple(hv_ColE.TupleGreater(hv_Width - 1)))).TupleOr(new HTuple(hv_ColE.TupleLess(0))) == 0)
                    {
                        if (new HTuple(hv_Direct.TupleEqual("inner")) != 0)
                        {
                            HOperatorSet.TupleAtan2(-hv_RowE + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                            hv_ATan = new HTuple(180).TupleRad() + hv_ATan;
                        }
                        else
                        {
                            HOperatorSet.TupleAtan2(-hv_RowE + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                        }
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowE, hv_ColE, hv_ATan, hv_DetectHeight / 2, hv_DetectWidth / 2);
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_Regions, ho_Rectangle, out ExpTmpOutVar_0);
                        ho_Regions.Dispose();
                        ho_Regions = ExpTmpOutVar_0;
                        if (new HTuple(hv_i.TupleEqual(0)) != 0)
                        {
                            hv_RowL2 = hv_RowE - hv_DetectHeight / 2 * hv_ATan.TupleSin();
                            hv_RowL3 = hv_RowE + hv_DetectHeight / 2 * hv_ATan.TupleSin();
                            hv_ColL2 = hv_ColE + hv_DetectHeight / 2 * hv_ATan.TupleCos();
                            hv_ColL3 = hv_ColE - hv_DetectHeight / 2 * hv_ATan.TupleCos();
                            ho_Arrow.Dispose();
                            HOperatorSet_Ex.gen_arrow_contour_xld(out ho_Arrow, hv_RowL3, hv_ColL3, hv_RowL2, hv_ColL2, 25, 25);
                            HOperatorSet.ConcatObj(ho_Regions, ho_Arrow, out ExpTmpOutVar_0);
                            ho_Regions.Dispose();
                            ho_Regions = ExpTmpOutVar_0;
                        }
                        HOperatorSet.GenMeasureRectangle2(hv_RowE, hv_ColE, hv_ATan, hv_DetectHeight / 2, hv_DetectWidth / 2, hv_Width, hv_Height, "nearest_neighbor", out hv_MsrHandle_Measure);
                        if (new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("negative")) != 0)
                        {
                            hv_Transition_COPY_INP_TMP = "negative";
                        }
                        else
                        {
                            if (new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("positive")) != 0)
                            {
                                hv_Transition_COPY_INP_TMP = "positive";
                            }
                            else
                            {
                                hv_Transition_COPY_INP_TMP = "all";
                            }
                        }
                        if (new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("first")) != 0)
                        {
                            hv_Select_COPY_INP_TMP = "first";
                        }
                        else
                        {
                            if (new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("last")) != 0)
                            {
                                hv_Select_COPY_INP_TMP = "last";
                            }
                            else
                            {
                                hv_Select_COPY_INP_TMP = "all";
                            }
                        }
                        HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure, hv_Sigma, hv_Threshold, hv_Transition_COPY_INP_TMP, hv_Select_COPY_INP_TMP, out hv_RowEdge, out hv_ColEdge, out hv_Amplitude, out hv_Distance);
                        HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);
                        hv_tRow = 0;
                        hv_tCol = 0;
                        hv_t = 0;
                        HOperatorSet.TupleLength(hv_RowEdge, out hv_Number);
                        if (new HTuple(hv_Number.TupleLess(1)) == 0)
                        {
                            HTuple end_val20 = hv_Number - 1;
                            HTuple step_val20 = 1;
                            hv_k = 0;
                            while (hv_k.Continue(end_val20, step_val20))
                            {
                                if (new HTuple(hv_Amplitude.TupleSelect(hv_k).TupleAbs().TupleGreater(hv_t)) != 0)
                                {
                                    hv_tRow = hv_RowEdge.TupleSelect(hv_k);
                                    hv_tCol = hv_ColEdge.TupleSelect(hv_k);
                                    hv_t = hv_Amplitude.TupleSelect(hv_k).TupleAbs();
                                }
                                hv_k = hv_k.TupleAdd(step_val20);
                            }
                            if (new HTuple(hv_t.TupleGreater(0)) != 0)
                            {
                                hv_ResultRow = hv_ResultRow.TupleConcat(hv_tRow);
                                hv_ResultColumn = hv_ResultColumn.TupleConcat(hv_tCol);
                            }
                        }
                    }
                    hv_i = hv_i.TupleAdd(step_val19);
                }
                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow.Dispose();
            }
        }
        public static void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem, HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            HTuple hv_Red = null;
            HTuple hv_Green = null;
            HTuple hv_Blue = null;
            HTuple hv_Row1Part = null;
            HTuple hv_Column1Part = null;
            HTuple hv_Row2Part = null;
            HTuple hv_Column2Part = null;
            HTuple hv_RowWin = null;
            HTuple hv_ColumnWin = null;
            HTuple hv_WidthWin = new HTuple();
            HTuple hv_HeightWin = null;
            HTuple hv_MaxAscent = null;
            HTuple hv_MaxDescent = null;
            HTuple hv_MaxWidth = null;
            HTuple hv_MaxHeight = null;
            HTuple hv_R = new HTuple();
            HTuple hv_C = new HTuple();
            HTuple hv_FactorRow = new HTuple();
            HTuple hv_FactorColumn = new HTuple();
            HTuple hv_UseShadow = null;
            HTuple hv_ShadowColor = null;
            HTuple hv_Exception = new HTuple();
            HTuple hv_Width = new HTuple();
            HTuple hv_Index = new HTuple();
            HTuple hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple();
            HTuple hv_W = new HTuple();
            HTuple hv_H = new HTuple();
            HTuple hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple();
            HTuple hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple();
            HTuple hv_DrawMode = new HTuple();
            HTuple hv_CurrentColor = new HTuple();
            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part, out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin, out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            if (new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1)) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if (new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1)) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if (new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple())) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            hv_String_COPY_INP_TMP = ("" + hv_String_COPY_INP_TMP + "").TupleSplit("\n");
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent, out hv_MaxWidth, out hv_MaxHeight);
            if (new HTuple(hv_CoordSystem.TupleEqual("window")) != 0)
            {
                hv_R = hv_Row_COPY_INP_TMP.Clone();
                hv_C = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                hv_FactorRow = 1.0 * hv_HeightWin / (hv_Row2Part - hv_Row1Part + 1);
                hv_FactorColumn = 1.0 * hv_WidthWin / (hv_Column2Part - hv_Column1Part + 1);
                hv_R = (hv_Row_COPY_INP_TMP - hv_Row1Part + 0.5) * hv_FactorRow;
                hv_C = (hv_Column_COPY_INP_TMP - hv_Column1Part + 0.5) * hv_FactorColumn;
            }
            hv_UseShadow = 1;
            hv_ShadowColor = "gray";
            if (new HTuple(hv_Box_COPY_INP_TMP.TupleSelect(0).TupleEqual("true")) != 0)
            {
                if (hv_Box_COPY_INP_TMP == null)
                {
                    hv_Box_COPY_INP_TMP = new HTuple();
                }
                hv_Box_COPY_INP_TMP[0] = "#fce9d4";
                hv_ShadowColor = "#f28d26";
            }
            if (new HTuple(new HTuple(hv_Box_COPY_INP_TMP.TupleLength()).TupleGreater(1)) != 0)
            {
                if (new HTuple(hv_Box_COPY_INP_TMP.TupleSelect(1).TupleEqual("true")) == 0)
                {
                    if (new HTuple(hv_Box_COPY_INP_TMP.TupleSelect(1).TupleEqual("false")) != 0)
                    {
                        hv_UseShadow = 0;
                    }
                    else
                    {
                        hv_ShadowColor = hv_Box_COPY_INP_TMP[1];
                        try
                        {
                            HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(1));
                        }
                        catch (HalconException HDevExpDefaultException)
                        {
                            HDevExpDefaultException.ToHTuple(out hv_Exception);
                            hv_Exception = "Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)";
                            throw new HalconException(hv_Exception);
                        }
                    }
                }
            }
            if (new HTuple(hv_Box_COPY_INP_TMP.TupleSelect(0).TupleNotEqual("false")) != 0)
            {
                try
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                }
                catch (HalconException HDevExpDefaultException)
                {
                    HDevExpDefaultException.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                hv_String_COPY_INP_TMP = " " + hv_String_COPY_INP_TMP + " ";
                hv_Width = new HTuple();
                hv_Index = 0;
                while (hv_Index <= new HTuple(hv_String_COPY_INP_TMP.TupleLength()) - 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                    hv_Index++;
                }
                hv_FrameHeight = hv_MaxHeight * new HTuple(hv_String_COPY_INP_TMP.TupleLength());
                hv_FrameWidth = new HTuple(0).TupleConcat(hv_Width).TupleMax();
                hv_R2 = hv_R + hv_FrameHeight;
                hv_C2 = hv_C + hv_FrameWidth;
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                HOperatorSet.SetColor(hv_WindowHandle, hv_ShadowColor);
                if (hv_UseShadow != 0)
                {
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R + 1, hv_C + 1, hv_R2 + 1, hv_C2 + 1);
                }
                HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R, hv_C, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            hv_Index = 0;
            while (hv_Index <= new HTuple(hv_String_COPY_INP_TMP.TupleLength()) - 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % new HTuple(hv_Color_COPY_INP_TMP.TupleLength()));
                if (new HTuple(hv_CurrentColor.TupleNotEqual("")).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual("auto"))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R + hv_MaxHeight * hv_Index;
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(hv_Index));
                hv_Index++;
            }
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part, hv_Column2Part);
        }
        public static void zoom_display(HWindow Hwin1, double mposition_row1, double mposition_col1, int Delta)
        {
            int current_beginRow;
            int current_beginCol;
            int current_endRow;
            int current_endCol;
            Hwin1.GetPart(out current_beginRow, out current_beginCol, out current_endRow, out current_endCol);
            int Ht = current_endRow - current_beginRow;
            int Wt = current_endCol - current_beginCol;
            if (Ht > 8)
            {
                if (Delta > 0)
                {
                    int zoom_beginRow = (int)((double)current_beginRow + (mposition_row1 - (double)current_beginRow) * 0.3);
                    int zoom_beginCol = (int)((double)current_beginCol + (mposition_col1 - (double)current_beginCol) * 0.3);
                    int zoom_endRow = (int)((double)current_endRow - ((double)current_endRow - mposition_row1) * 0.3);
                    int zoom_endCol = (int)((double)current_endCol - ((double)current_endCol - mposition_col1) * 0.3);
                    Hwin1.SetPart(zoom_beginRow, zoom_beginCol, zoom_endRow, zoom_endCol);
                    Hwin1.ClearWindow();
                }
            }
            if (Ht < 8000)
            {
                if (Delta < 0)
                {
                    int zoom_beginRow = (int)(mposition_row1 - (mposition_row1 - (double)current_beginRow) / 0.7);
                    int zoom_beginCol = (int)(mposition_col1 - (mposition_col1 - (double)current_beginCol) / 0.7);
                    int zoom_endRow = (int)(mposition_row1 + ((double)current_endRow - mposition_row1) / 0.7);
                    int zoom_endCol = (int)(mposition_col1 + ((double)current_endCol - mposition_col1) / 0.7);
                    Hwin1.SetPart(zoom_beginRow, zoom_beginCol, zoom_endRow, zoom_endCol);
                    Hwin1.ClearWindow();
                }
            }
        }
        public static void pts_to_best_line(out HObject ho_Line, HTuple hv_Rows, HTuple hv_Cols, HTuple hv_ActiveNum, out HTuple hv_Row1, out HTuple hv_Col1, out HTuple hv_Row2, out HTuple hv_Col2)
        {
            HObject ho_Contour = null;
            HTuple hv_Length = null;
            HTuple hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple();
            HTuple hv_Dist = new HTuple();
            HTuple hv_Length2 = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            hv_Row1 = null;
            hv_Col1 = null;
            hv_Row2 = null;
            hv_Col2 = null;
            ho_Line.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.TupleLength(hv_Cols, out hv_Length);
            if (new HTuple(hv_Length.TupleGreaterEqual(hv_ActiveNum)).TupleAnd(new HTuple(hv_ActiveNum.TupleGreater(1))) != 0)
            {
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                HOperatorSet.FitLineContourXld(ho_Contour, "tukey", hv_ActiveNum, 0, 5, 2, out hv_Row1, out hv_Col1, out hv_Row2, out hv_Col2, out hv_Nr, out hv_Nc, out hv_Dist);
                HOperatorSet.TupleLength(hv_Dist, out hv_Length2);
                if (new HTuple(hv_Length2.TupleLess(1)) != 0)
                {
                    ho_Contour.Dispose();
                    return;
                }
                ho_Line.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Line, hv_Row1.TupleConcat(hv_Row2), hv_Col1.TupleConcat(hv_Col2));
            }
            ho_Contour.Dispose();
        }
        public static void get_shape_model_contour_ref(HObject ho_ModelRegion, out HObject ho_ModelContoursTrans, HTuple hv_ModelID)
        {
            HTuple hv_ExpDefaultCtrlDummyVar = null;
            HTuple hv_Row = null;
            HTuple hv_Column = null;
            HTuple hv_HomMat2DIdentity = null;
            HTuple hv_HomMat2DTranslate = null;
            HOperatorSet.GenEmptyObj(out ho_ModelContoursTrans);
            HObject ho_ModelContours;
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.AreaCenter(ho_ModelRegion, out hv_ExpDefaultCtrlDummyVar, out hv_Row, out hv_Column);
            ho_ModelContours.Dispose();
            HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
            HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
            HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row, hv_Column, out hv_HomMat2DTranslate);
            ho_ModelContoursTrans.Dispose();
            HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ModelContoursTrans, hv_HomMat2DTranslate);
            ho_ModelContours.Dispose();
        }
        public static void dev_display_profile_line(HTuple hv_WindowHandle, HTuple hv_Row, HTuple hv_Col, HTuple hv_Phi, HTuple hv_Length1, HTuple hv_Length2)
        {
            HObject ho_CProfile;
            HOperatorSet.GenEmptyObj(out ho_CProfile);
            HObject ho_CArrow;
            HOperatorSet.GenEmptyObj(out ho_CArrow);
            HObject ho_CMidPoint;
            HOperatorSet.GenEmptyObj(out ho_CMidPoint);
            HTuple hv_ArrowAngle = new HTuple(45).TupleRad() / 2;
            HTuple hv_ArrowLength = hv_Length1 * 0.2;
            HTuple hv_PSize = hv_Length2 * 0.2;
            HTuple hv_RowStart = hv_Row + hv_Phi.TupleSin() * hv_Length1;
            HTuple hv_RowEnd = hv_Row - hv_Phi.TupleSin() * hv_Length1;
            HTuple hv_ColStart = hv_Col - hv_Phi.TupleCos() * hv_Length1;
            HTuple hv_ColEnd = hv_Col + hv_Phi.TupleCos() * hv_Length1;
            ho_CProfile.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_CProfile, hv_RowStart.TupleConcat(hv_RowEnd), hv_ColStart.TupleConcat(hv_ColEnd));
            ho_CArrow.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_CArrow, (hv_RowEnd - (hv_ArrowAngle - hv_Phi).TupleSin() * hv_ArrowLength).TupleConcat(hv_RowEnd).TupleConcat(hv_RowEnd + (hv_ArrowAngle + hv_Phi).TupleSin() * hv_ArrowLength), (hv_ColEnd - (hv_ArrowAngle - hv_Phi).TupleCos() * hv_ArrowLength).TupleConcat(hv_ColEnd).TupleConcat(hv_ColEnd - (hv_ArrowAngle + hv_Phi).TupleCos() * hv_ArrowLength));
            ho_CMidPoint.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_CMidPoint, (hv_Row - (new HTuple(90).TupleRad() + hv_Phi).TupleSin() * hv_PSize).TupleConcat(hv_Row - (hv_Phi - new HTuple(90).TupleRad()).TupleSin() * hv_PSize), (hv_Col + (new HTuple(90).TupleRad() + hv_Phi).TupleCos() * hv_PSize).TupleConcat(hv_Col + (hv_Phi - new HTuple(90).TupleRad()).TupleCos() * hv_PSize));
            HOperatorSet.SetLineWidth(hv_WindowHandle, 3);
            HOperatorSet.SetColor(hv_WindowHandle, "white");
            HOperatorSet.DispObj(ho_CProfile, hv_WindowHandle);
            HOperatorSet.DispObj(ho_CArrow, hv_WindowHandle);
            HOperatorSet.SetLineWidth(hv_WindowHandle, 1);
            HOperatorSet.SetColor(hv_WindowHandle, "blue");
            HOperatorSet.DispObj(ho_CProfile, hv_WindowHandle);
            HOperatorSet.DispObj(ho_CArrow, hv_WindowHandle);
            ho_CProfile.Dispose();
            ho_CArrow.Dispose();
            ho_CMidPoint.Dispose();
        }
        public static void draw_rake(out HObject ho_Regions, HTuple hv_WindowHandle, HTuple hv_Elements, HTuple hv_DetectHeight, HTuple hv_DetectWidth, out HTuple hv_Row1, out HTuple hv_Column1, out HTuple hv_Row2, out HTuple hv_Column2)
        {
            HObject[] OTemp = new HObject[20];
            HObject ho_Rectangle = null;
            HObject ho_Arrow = null;
            HTuple hv_ATan = null;
            HTuple hv_Deg = null;
            HTuple hv_Deg2 = null;
            HTuple hv_RowC = new HTuple();
            HTuple hv_ColC = new HTuple();
            HTuple hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple();
            HTuple hv_RowL3 = new HTuple();
            HTuple hv_ColL2 = new HTuple();
            HTuple hv_ColL3 = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HObject ho_RegionLines;
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            disp_message(hv_WindowHandle, "点击鼠标左键画一条直线,点击右键确认", "window", 12, 12, "red", "false");
            ho_Regions.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.DrawLine(hv_WindowHandle, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);
            ho_RegionLines.Dispose();
            HOperatorSet.GenRegionLine(out ho_RegionLines, hv_Row1, hv_Column1, hv_Row2, hv_Column2);
            HObject ExpTmpOutVar_0;
            HOperatorSet.ConcatObj(ho_Regions, ho_RegionLines, out ExpTmpOutVar_0);
            ho_Regions.Dispose();
            ho_Regions = ExpTmpOutVar_0;
            HOperatorSet.TupleAtan2(-hv_Row2 + hv_Row1, hv_Column2 - hv_Column1, out hv_ATan);
            HOperatorSet.TupleDeg(hv_ATan, out hv_Deg);
            hv_ATan += new HTuple(90).TupleRad();
            HOperatorSet.TupleDeg(hv_ATan, out hv_Deg2);
            HTuple step_val14 = 1;
            HTuple hv_i = 1;
            while (hv_i.Continue(hv_Elements, step_val14))
            {
                hv_RowC = hv_Row1 + (hv_Row2 - hv_Row1) * hv_i / (hv_Elements + 1);
                hv_ColC = hv_Column1 + (hv_Column2 - hv_Column1) * hv_i / (hv_Elements + 1);
                if (new HTuple(hv_Elements.TupleEqual(1)) != 0)
                {
                    HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC, hv_Deg2.TupleRad(), hv_DetectHeight / 2, hv_Distance / 2);
                }
                else
                {
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC, hv_Deg2.TupleRad(), hv_DetectHeight / 2, hv_DetectWidth / 2);
                }
                HOperatorSet.ConcatObj(ho_Regions, ho_Rectangle, out ExpTmpOutVar_0);
                ho_Regions.Dispose();
                ho_Regions = ExpTmpOutVar_0;
                if (new HTuple(hv_i.TupleEqual(1)) != 0)
                {
                    hv_RowL2 = hv_RowC + hv_DetectHeight / 2 * (-hv_ATan).TupleSin();
                    hv_RowL3 = hv_RowC - hv_DetectHeight / 2 * (-hv_ATan).TupleSin();
                    hv_ColL2 = hv_ColC + hv_DetectHeight / 2 * (-hv_ATan).TupleCos();
                    hv_ColL3 = hv_ColC - hv_DetectHeight / 2 * (-hv_ATan).TupleCos();
                    ho_Arrow.Dispose();
                    HOperatorSet_Ex.gen_arrow_contour_xld(out ho_Arrow, hv_RowL3, hv_ColL3, hv_RowL2, hv_ColL2, 25, 25);
                    HOperatorSet.ConcatObj(ho_Regions, ho_Arrow, out ExpTmpOutVar_0);
                    ho_Regions.Dispose();
                    ho_Regions = ExpTmpOutVar_0;
                }
                hv_i = hv_i.TupleAdd(step_val14);
            }
            ho_RegionLines.Dispose();
            ho_Rectangle.Dispose();
            ho_Arrow.Dispose();
        }
        public static void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font, HTuple hv_Bold, HTuple hv_Slant)
        {
            HTuple hv_OS = null;
            HTuple hv_Exception = new HTuple();
            HTuple hv_BoldString = new HTuple();
            HTuple hv_SlantString = new HTuple();
            HTuple hv_AllowedFontSizes = new HTuple();
            HTuple hv_Distances = new HTuple();
            HTuple hv_Indices = new HTuple();
            HTuple hv_Fonts = new HTuple();
            HTuple hv_FontSelRegexp = new HTuple();
            HTuple hv_FontsCourier = new HTuple();
            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            if (new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple())).TupleOr(new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if (new HTuple(hv_OS.TupleSubstr(0, 2).TupleEqual("Win")) != 0)
            {
                if (new HTuple(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono")).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier")))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Courier New";
                }
                else
                {
                    if (new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans")) != 0)
                    {
                        hv_Font_COPY_INP_TMP = "Arial";
                    }
                    else
                    {
                        if (new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif")) != 0)
                        {
                            hv_Font_COPY_INP_TMP = "Times New Roman";
                        }
                    }
                }
                if (new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true")) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 1;
                }
                else
                {
                    if (new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false")) == 0)
                    {
                        hv_Exception = "Wrong value of control parameter Bold";
                        throw new HalconException(hv_Exception);
                    }
                    hv_Bold_COPY_INP_TMP = 0;
                }
                if (new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true")) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 1;
                }
                else
                {
                    if (new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false")) == 0)
                    {
                        hv_Exception = "Wrong value of control parameter Slant";
                        throw new HalconException(hv_Exception);
                    }
                    hv_Slant_COPY_INP_TMP = 0;
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, "-" + hv_Font_COPY_INP_TMP + "-" + hv_Size_COPY_INP_TMP + "-*-" + hv_Slant_COPY_INP_TMP + "-*-*-" + hv_Bold_COPY_INP_TMP + "-");
                }
                catch (HalconException HDevExpDefaultException)
                {
                    HDevExpDefaultException.ToHTuple(out hv_Exception);
                }
            }
            else
            {
                if (new HTuple(hv_OS.TupleSubstr(0, 2).TupleEqual("Dar")) != 0)
                {
                    if (new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true")) != 0)
                    {
                        hv_BoldString = "Bold";
                    }
                    else
                    {
                        if (new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false")) == 0)
                        {
                            hv_Exception = "Wrong value of control parameter Bold";
                            throw new HalconException(hv_Exception);
                        }
                        hv_BoldString = "";
                    }
                    if (new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true")) != 0)
                    {
                        hv_SlantString = "Italic";
                    }
                    else
                    {
                        if (new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false")) == 0)
                        {
                            hv_Exception = "Wrong value of control parameter Slant";
                            throw new HalconException(hv_Exception);
                        }
                        hv_SlantString = "";
                    }
                    if (new HTuple(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono")).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier")))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier"))) != 0)
                    {
                        hv_Font_COPY_INP_TMP = "CourierNewPS";
                    }
                    else
                    {
                        if (new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans")) != 0)
                        {
                            hv_Font_COPY_INP_TMP = "Arial";
                        }
                        else
                        {
                            if (new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif")) != 0)
                            {
                                hv_Font_COPY_INP_TMP = "TimesNewRomanPS";
                            }
                        }
                    }
                    if (new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true")).TupleOr(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                    {
                        hv_Font_COPY_INP_TMP = hv_Font_COPY_INP_TMP + "-" + hv_BoldString + hv_SlantString;
                    }
                    hv_Font_COPY_INP_TMP += "MT";
                    try
                    {
                        HOperatorSet.SetFont(hv_WindowHandle, hv_Font_COPY_INP_TMP + "-" + hv_Size_COPY_INP_TMP);
                    }
                    catch (HalconException HDevExpDefaultException)
                    {
                        HDevExpDefaultException.ToHTuple(out hv_Exception);
                    }
                }
                else
                {
                    hv_Size_COPY_INP_TMP *= 1.25;
                    hv_AllowedFontSizes = new HTuple();
                    hv_AllowedFontSizes[0] = 11;
                    hv_AllowedFontSizes[1] = 14;
                    hv_AllowedFontSizes[2] = 17;
                    hv_AllowedFontSizes[3] = 20;
                    hv_AllowedFontSizes[4] = 25;
                    hv_AllowedFontSizes[5] = 34;
                    if (new HTuple(hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP).TupleEqual(-1)) != 0)
                    {
                        hv_Distances = (hv_AllowedFontSizes - hv_Size_COPY_INP_TMP).TupleAbs();
                        HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
                        hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(0));
                    }
                    if (new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono")).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))) != 0)
                    {
                        hv_Font_COPY_INP_TMP = "courier";
                    }
                    else
                    {
                        if (new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans")) != 0)
                        {
                            hv_Font_COPY_INP_TMP = "helvetica";
                        }
                        else
                        {
                            if (new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif")) != 0)
                            {
                                hv_Font_COPY_INP_TMP = "times";
                            }
                        }
                    }
                    if (new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true")) != 0)
                    {
                        hv_Bold_COPY_INP_TMP = "bold";
                    }
                    else
                    {
                        if (new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false")) == 0)
                        {
                            hv_Exception = "Wrong value of control parameter Bold";
                            throw new HalconException(hv_Exception);
                        }
                        hv_Bold_COPY_INP_TMP = "medium";
                    }
                    if (new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true")) != 0)
                    {
                        if (new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times")) != 0)
                        {
                            hv_Slant_COPY_INP_TMP = "i";
                        }
                        else
                        {
                            hv_Slant_COPY_INP_TMP = "o";
                        }
                    }
                    else
                    {
                        if (new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false")) == 0)
                        {
                            hv_Exception = "Wrong value of control parameter Slant";
                            throw new HalconException(hv_Exception);
                        }
                        hv_Slant_COPY_INP_TMP = "r";
                    }
                    try
                    {
                        HOperatorSet.SetFont(hv_WindowHandle, "-adobe-" + hv_Font_COPY_INP_TMP + "-" + hv_Bold_COPY_INP_TMP + "-" + hv_Slant_COPY_INP_TMP + "-normal-*-" + hv_Size_COPY_INP_TMP + "-*-*-*-*-*-*-*");
                    }
                    catch (HalconException HDevExpDefaultException)
                    {
                        HDevExpDefaultException.ToHTuple(out hv_Exception);
                        if (new HTuple(hv_OS.TupleSubstr(0, 4).TupleEqual("Linux")).TupleAnd(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier"))) != 0)
                        {
                            HOperatorSet.QueryFont(hv_WindowHandle, out hv_Fonts);
                            hv_FontSelRegexp = "^-[^-]*-[^-]*[Cc]ourier[^-]*-" + hv_Bold_COPY_INP_TMP + "-" + hv_Slant_COPY_INP_TMP;
                            hv_FontsCourier = hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp).TupleRegexpMatch(hv_FontSelRegexp);
                            if (new HTuple(new HTuple(hv_FontsCourier.TupleLength()).TupleEqual(0)) != 0)
                            {
                                hv_Exception = "Wrong font name";
                            }
                            else
                            {
                                try
                                {
                                    HOperatorSet.SetFont(hv_WindowHandle, hv_FontsCourier.TupleSelect(0) + "-normal-*-" + hv_Size_COPY_INP_TMP + "-*-*-*-*-*-*-*");
                                }
                                catch (HalconException HDevExpDefaultException2)
                                {
                                    HDevExpDefaultException2.ToHTuple(out hv_Exception);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void peak(HObject ho_Image, HTuple hv_Row, HTuple hv_Coloumn, HTuple hv_Phi, HTuple hv_Length1, HTuple hv_Length2, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold, HTuple hv_Transition, HTuple hv_Select, out HTuple hv_EdgeRows, out HTuple hv_EdgeColumns, out HTuple hv_ResultRow, out HTuple hv_ResultColumn)
        {
            HTuple hv_Cos = null;
            HTuple hv_Sin = null;
            HTuple hv_ResultRows = null;
            HTuple hv_ResultColumns = null;
            HTuple hv_i = new HTuple();
            HTuple hv_Distance = new HTuple();
            HObject ho_Regions;
            HOperatorSet.GenEmptyObj(out ho_Regions);
            hv_ResultColumn = new HTuple();
            hv_ResultRow = -9999;
            HTuple hv_ResultCol = -9999;
            hv_EdgeColumns = new HTuple();
            hv_EdgeRows = new HTuple();
            HTuple hv_ROILineRow = 0;
            HTuple hv_ROILineCol = 0;
            HTuple hv_ROILineRow2 = 0;
            HTuple hv_ROILineCol2 = 0;
            HTuple hv_StdLineRow = 0;
            HTuple hv_StdLineCol = 0;
            HTuple hv_StdLineRow2 = 0;
            HTuple hv_StdLineCol2 = 0;
            if (new HTuple(hv_Length1.TupleLessEqual(0)).TupleOr(new HTuple(hv_Length2.TupleLessEqual(0))) != 0)
            {
                ho_Regions.Dispose();
            }
            else
            {
                HOperatorSet.TupleCos(hv_Phi, out hv_Cos);
                HOperatorSet.TupleSin(hv_Phi, out hv_Sin);
                HTuple hv_Col = 1.0 * (hv_Coloumn - hv_Length1 * hv_Cos - hv_Length2 * hv_Sin);
                HTuple hv_Row2 = 1.0 * (hv_Row - (-hv_Length1 * hv_Sin + hv_Length2 * hv_Cos));
                HTuple hv_Col2 = 1.0 * (hv_Coloumn + hv_Length1 * hv_Cos - hv_Length2 * hv_Sin);
                HTuple hv_Row3 = 1.0 * (hv_Row - (hv_Length1 * hv_Sin + hv_Length2 * hv_Cos));
                HTuple hv_Col3 = 1.0 * (hv_Coloumn + hv_Length1 * hv_Cos + hv_Length2 * hv_Sin);
                HTuple hv_Row4 = 1.0 * (hv_Row - (hv_Length1 * hv_Sin - hv_Length2 * hv_Cos));
                HTuple hv_Col4 = 1.0 * (hv_Coloumn - hv_Length1 * hv_Cos + hv_Length2 * hv_Sin);
                HTuple hv_Row5 = 1.0 * (hv_Row - (-hv_Length1 * hv_Sin - hv_Length2 * hv_Cos));
                hv_StdLineRow = hv_Row3.Clone();
                hv_StdLineCol = hv_Col2.Clone();
                hv_StdLineRow2 = hv_Row4.Clone();
                hv_StdLineCol2 = hv_Col3.Clone();
                hv_ROILineRow = (hv_Row2 + hv_Row3) * 0.5;
                hv_ROILineCol = (hv_Col + hv_Col2) * 0.5;
                hv_ROILineRow2 = (hv_Row4 + hv_Row5) * 0.5;
                hv_ROILineCol2 = (hv_Col3 + hv_Col4) * 0.5;
                ho_Regions.Dispose();
                HOperatorSet_Ex.rake(ho_Image, out ho_Regions, 1.0 * hv_Length2 * 2, hv_Length1 * 2, hv_DetectWidth, hv_Sigma, hv_Threshold, hv_Transition, hv_Select, hv_ROILineRow, hv_ROILineCol, hv_ROILineRow2, hv_ROILineCol2, out hv_ResultRows, out hv_ResultColumns);
                HTuple hv_Max = 0;
                if (new HTuple(new HTuple(hv_ResultColumns.TupleLength()).TupleGreater(0)) != 0)
                {
                    hv_EdgeRows = hv_ResultRows.Clone();
                    hv_EdgeColumns = hv_ResultColumns.Clone();
                    hv_i = 0;
                    while (hv_i <= new HTuple(hv_ResultColumns.TupleLength()) - 1)
                    {
                        HOperatorSet.DistancePl(hv_ResultRows.TupleSelect(hv_i), hv_ResultColumns.TupleSelect(hv_i), hv_StdLineRow, hv_StdLineCol, hv_StdLineRow2, hv_StdLineCol2, out hv_Distance);
                        if (new HTuple(hv_Max.TupleLess(hv_Distance)) != 0)
                        {
                            hv_Max = hv_Distance.Clone();
                            hv_ResultRow = hv_ResultRows.TupleSelect(hv_i);
                            hv_ResultColumn = hv_ResultColumns.TupleSelect(hv_i);
                        }
                        hv_i++;
                    }
                }
                ho_Regions.Dispose();
            }
        }
        public static void pts_to_best_circle(out HObject ho_Circle, HTuple hv_Rows, HTuple hv_Cols, HTuple hv_ActiveNum, HTuple hv_ArcType, out HTuple hv_RowCenter, out HTuple hv_ColCenter, out HTuple hv_Radius)
        {
            HObject ho_Contour = null;
            HTuple hv_Length = null;
            HTuple hv_StartPhi = new HTuple();
            HTuple hv_EndPhi = new HTuple();
            HTuple hv_PointOrder = new HTuple();
            HTuple hv_Length2 = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            hv_RowCenter = null;
            hv_ColCenter = null;
            hv_Radius = null;
            ho_Circle.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.TupleLength(hv_Cols, out hv_Length);
            if (new HTuple(hv_Length.TupleGreaterEqual(hv_ActiveNum)).TupleAnd(new HTuple(hv_ActiveNum.TupleGreater(2))) != 0)
            {
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                HOperatorSet.FitCircleContourXld(ho_Contour, "geotukey", -1, 0, 0, 3, 2, out hv_RowCenter, out hv_ColCenter, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                HOperatorSet.TupleLength(hv_StartPhi, out hv_Length2);
                if (new HTuple(hv_Length2.TupleLess(1)) != 0)
                {
                    ho_Contour.Dispose();
                    return;
                }
                if (new HTuple(hv_ArcType.TupleEqual("arc")) != 0)
                {
                    ho_Circle.Dispose();
                    HOperatorSet.GenCircleContourXld(out ho_Circle, hv_RowCenter, hv_ColCenter, hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder, 1);
                }
                else
                {
                    ho_Circle.Dispose();
                    HOperatorSet.GenCircleContourXld(out ho_Circle, hv_RowCenter, hv_ColCenter, hv_Radius, 0, new HTuple(360).TupleRad(), hv_PointOrder, 1);
                }
            }
            ho_Contour.Dispose();
        }
        public static void rake(HObject ho_Image, out HObject ho_Regions, HTuple hv_Elements, HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold, HTuple hv_Transition, HTuple hv_Select, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2, HTuple hv_Column2, out HTuple hv_ResultRow, out HTuple hv_ResultColumn)
        {
            HObject[] OTemp = new HObject[20];
            HObject ho_Rectangle = null;
            HObject ho_Arrow = null;
            HTuple hv_Width = null;
            HTuple hv_Height = null;
            HTuple hv_ATan = null;
            HTuple hv_Deg = null;
            HTuple hv_Deg2 = null;
            HTuple hv_RowC = new HTuple();
            HTuple hv_ColC = new HTuple();
            HTuple hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple();
            HTuple hv_RowL3 = new HTuple();
            HTuple hv_ColL2 = new HTuple();
            HTuple hv_ColL3 = new HTuple();
            HTuple hv_MsrHandle_Measure = new HTuple();
            HTuple hv_RowEdge = new HTuple();
            HTuple hv_ColEdge = new HTuple();
            HTuple hv_Amplitude = new HTuple();
            HTuple hv_tRow = new HTuple();
            HTuple hv_tCol = new HTuple();
            HTuple hv_t = new HTuple();
            HTuple hv_Number = new HTuple();
            HTuple hv_j = new HTuple();
            HTuple hv_Select_COPY_INP_TMP = hv_Select.Clone();
            HTuple hv_Transition_COPY_INP_TMP = hv_Transition.Clone();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            ho_Regions.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();
            HOperatorSet.TupleAtan2(-hv_Row2 + hv_Row1, hv_Column2 - hv_Column1, out hv_ATan);
            HOperatorSet.TupleDeg(hv_ATan, out hv_Deg);
            hv_ATan += new HTuple(90).TupleRad();
            HOperatorSet.TupleDeg(hv_ATan, out hv_Deg2);
            HTuple step_val13 = 1;
            HTuple hv_i = 1;
            while (hv_i.Continue(hv_Elements, step_val13))
            {
                hv_RowC = hv_Row1 + (hv_Row2 - hv_Row1) * hv_i / (hv_Elements + 1);
                hv_ColC = hv_Column1 + (hv_Column2 - hv_Column1) * hv_i / (hv_Elements + 1);
                if (new HTuple(new HTuple(new HTuple(hv_RowC.TupleGreater(hv_Height - 1)).TupleOr(new HTuple(hv_RowC.TupleLess(0)))).TupleOr(new HTuple(hv_ColC.TupleGreater(hv_Width - 1)))).TupleOr(new HTuple(hv_ColC.TupleLess(0))) == 0)
                {
                    if (new HTuple(hv_Elements.TupleEqual(1)) != 0)
                    {
                        HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC, hv_Deg2.TupleRad(), hv_DetectHeight / 2, hv_Distance / 2);
                    }
                    else
                    {
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC, hv_Deg2.TupleRad(), hv_DetectHeight / 2, hv_DetectWidth / 2);
                    }
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Regions, ho_Rectangle, out ExpTmpOutVar_0);
                    ho_Regions.Dispose();
                    ho_Regions = ExpTmpOutVar_0;
                    if (new HTuple(hv_i.TupleEqual(1)) != 0)
                    {
                        hv_RowL2 = hv_RowC + hv_DetectHeight / 2 * (-hv_ATan).TupleSin();
                        hv_RowL3 = hv_RowC - hv_DetectHeight / 2 * (-hv_ATan).TupleSin();
                        hv_ColL2 = hv_ColC + hv_DetectHeight / 2 * (-hv_ATan).TupleCos();
                        hv_ColL3 = hv_ColC - hv_DetectHeight / 2 * (-hv_ATan).TupleCos();
                        ho_Arrow.Dispose();
                        HOperatorSet_Ex.gen_arrow_contour_xld(out ho_Arrow, hv_RowL3, hv_ColL3, hv_RowL2, hv_ColL2, 25, 25);
                        HOperatorSet.ConcatObj(ho_Regions, ho_Arrow, out ExpTmpOutVar_0);
                        ho_Regions.Dispose();
                        ho_Regions = ExpTmpOutVar_0;
                    }
                    HOperatorSet.GenMeasureRectangle2(hv_RowC, hv_ColC, hv_Deg2.TupleRad(), hv_DetectHeight / 2, hv_DetectWidth / 2, hv_Width, hv_Height, "nearest_neighbor", out hv_MsrHandle_Measure);
                    if (new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("negative")) != 0)
                    {
                        hv_Transition_COPY_INP_TMP = "negative";
                    }
                    else
                    {
                        if (new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("positive")) != 0)
                        {
                            hv_Transition_COPY_INP_TMP = "positive";
                        }
                        else
                        {
                            hv_Transition_COPY_INP_TMP = "all";
                        }
                    }
                    if (new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("first")) != 0)
                    {
                        hv_Select_COPY_INP_TMP = "first";
                    }
                    else
                    {
                        if (new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("last")) != 0)
                        {
                            hv_Select_COPY_INP_TMP = "last";
                        }
                        else
                        {
                            hv_Select_COPY_INP_TMP = "all";
                        }
                    }
                    HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure, hv_Sigma, hv_Threshold, hv_Transition_COPY_INP_TMP, hv_Select_COPY_INP_TMP, out hv_RowEdge, out hv_ColEdge, out hv_Amplitude, out hv_Distance);
                    HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);
                    hv_tRow = 0;
                    hv_tCol = 0;
                    hv_t = 0;
                    HOperatorSet.TupleLength(hv_RowEdge, out hv_Number);
                    if (new HTuple(hv_Number.TupleLess(1)) == 0)
                    {
                        HTuple end_val69 = hv_Number - 1;
                        HTuple step_val14 = 1;
                        hv_j = 0;
                        while (hv_j.Continue(end_val69, step_val14))
                        {
                            if (new HTuple(hv_Amplitude.TupleSelect(hv_j).TupleAbs().TupleGreater(hv_t)) != 0)
                            {
                                hv_tRow = hv_RowEdge.TupleSelect(hv_j);
                                hv_tCol = hv_ColEdge.TupleSelect(hv_j);
                                hv_t = hv_Amplitude.TupleSelect(hv_j).TupleAbs();
                            }
                            hv_j = hv_j.TupleAdd(step_val14);
                        }
                        if (new HTuple(hv_t.TupleGreater(0)) != 0)
                        {
                            hv_ResultRow = hv_ResultRow.TupleConcat(hv_tRow);
                            hv_ResultColumn = hv_ResultColumn.TupleConcat(hv_tCol);
                        }
                    }
                }
                hv_i = hv_i.TupleAdd(step_val13);
            }
            HOperatorSet.TupleLength(hv_ResultRow, out hv_Number);
            ho_Rectangle.Dispose();
            ho_Arrow.Dispose();
        }
        public static void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {
            HObject[] OTemp = new HObject[20];
            HObject ho_TempArrow = null;
            HTuple hv_Length = null;
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            HTuple hv_ZeroLengthIndices = hv_Length.TupleFind(0);
            if (new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1)) != 0)
            {
                if (hv_Length == null)
                {
                    hv_Length = new HTuple();
                }
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            HTuple hv_DR = 1.0 * (hv_Row2 - hv_Row1) / hv_Length;
            HTuple hv_DC = 1.0 * (hv_Column2 - hv_Column1) / hv_Length;
            HTuple hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            HTuple hv_RowP = hv_Row1 + (hv_Length - hv_HeadLength) * hv_DR + hv_HalfHeadWidth * hv_DC;
            HTuple hv_ColP = hv_Column1 + (hv_Length - hv_HeadLength) * hv_DC - hv_HalfHeadWidth * hv_DR;
            HTuple hv_RowP2 = hv_Row1 + (hv_Length - hv_HeadLength) * hv_DR - hv_HalfHeadWidth * hv_DC;
            HTuple hv_ColP2 = hv_Column1 + (hv_Length - hv_HeadLength) * hv_DC + hv_HalfHeadWidth * hv_DR;
            HTuple hv_Index = 0;
            while (hv_Index <= new HTuple(hv_Length.TupleLength()) - 1)
            {
                if (new HTuple(hv_Length.TupleSelect(hv_Index).TupleEqual(-1)) != 0)
                {
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index), hv_Column1.TupleSelect(hv_Index));
                }
                else
                {
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index).TupleConcat(hv_Row2.TupleSelect(hv_Index)).TupleConcat(hv_RowP.TupleSelect(hv_Index)).TupleConcat(hv_Row2.TupleSelect(hv_Index)).TupleConcat(hv_RowP2.TupleSelect(hv_Index)).TupleConcat(hv_Row2.TupleSelect(hv_Index)), hv_Column1.TupleSelect(hv_Index).TupleConcat(hv_Column2.TupleSelect(hv_Index)).TupleConcat(hv_ColP.TupleSelect(hv_Index)).TupleConcat(hv_Column2.TupleSelect(hv_Index)).TupleConcat(hv_ColP2.TupleSelect(hv_Index)).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                }
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                ho_Arrow.Dispose();
                ho_Arrow = ExpTmpOutVar_0;
                hv_Index++;
            }
            ho_TempArrow.Dispose();
        }
        public static void draw_spoke(HObject ho_Image, out HObject ho_Regions, HTuple hv_WindowHandle, HTuple hv_Elements, HTuple hv_DetectHeight, HTuple hv_DetectWidth, out HTuple hv_ROIRows, out HTuple hv_ROICols, out HTuple hv_Direct)
        {
            HObject[] OTemp = new HObject[20];
            HObject ho_Rectangle = null;
            HObject ho_Arrow = null;
            HTuple hv_Rows = null;
            HTuple hv_Cols = null;
            HTuple hv_Weights = null;
            HTuple hv_Length = null;
            HTuple hv_RowC = null;
            HTuple hv_ColumnC = null;
            HTuple hv_Radius = null;
            HTuple hv_StartPhi = null;
            HTuple hv_EndPhi = null;
            HTuple hv_PointOrder = null;
            HTuple hv_RowXLD = null;
            HTuple hv_ColXLD = null;
            HTuple hv_Row = null;
            HTuple hv_Column = null;
            HTuple hv_Row2 = null;
            HTuple hv_Column2 = null;
            HTuple hv_DistanceStart = null;
            HTuple hv_DistanceEnd = null;
            HTuple hv_Length2 = null;
            HTuple hv_Length3 = null;
            HTuple hv_j = new HTuple();
            HTuple hv_RowE = new HTuple();
            HTuple hv_ColE = new HTuple();
            HTuple hv_ATan = new HTuple();
            HTuple hv_RowL2 = new HTuple();
            HTuple hv_RowL3 = new HTuple();
            HTuple hv_ColL2 = new HTuple();
            HTuple hv_ColL3 = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HObject ho_ContOut;
            HOperatorSet.GenEmptyObj(out ho_ContOut);
            HObject ho_Contour;
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HObject ho_ContCircle;
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HObject ho_Cross;
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            hv_ROIRows = new HTuple();
            hv_ROICols = new HTuple();
            hv_Direct = new HTuple();
            HOperatorSet_Ex.disp_message(hv_WindowHandle, "1、画4个以上点确定一个圆弧,点击右键确认", "window", 12, 12, "red", "false");
            ho_Regions.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            ho_ContOut.Dispose();
            HOperatorSet.DrawNurbs(out ho_ContOut, hv_WindowHandle, "true", "true", "true", "true", 3, out hv_Rows, out hv_Cols, out hv_Weights);
            HOperatorSet.TupleLength(hv_Weights, out hv_Length);
            if (new HTuple(hv_Length.TupleLess(4)) != 0)
            {
                HOperatorSet_Ex.disp_message(hv_WindowHandle, "提示：点数太少，请重画", "window", 32, 12, "red", "false");
                hv_ROIRows = new HTuple();
                hv_ROICols = new HTuple();
                ho_ContOut.Dispose();
                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Cross.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow.Dispose();
            }
            else
            {
                hv_ROIRows = hv_Rows.Clone();
                hv_ROICols = hv_Cols.Clone();
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                HOperatorSet.FitCircleContourXld(ho_Contour, "algebraic", -1, 0, 0, 3, 2, out hv_RowC, out hv_ColumnC, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                ho_ContCircle.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_RowC, hv_ColumnC, hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder, 3);
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_Regions, ho_ContCircle, out ExpTmpOutVar_0);
                ho_Regions.Dispose();
                ho_Regions = ExpTmpOutVar_0;
                HOperatorSet.GetContourXld(ho_ContCircle, out hv_RowXLD, out hv_ColXLD);
                HOperatorSet.DispObj(ho_Image, hv_WindowHandle);
                HOperatorSet.DispObj(ho_ContCircle, hv_WindowHandle);
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_RowC, hv_ColumnC, 60, 0.785398);
                HOperatorSet.DispObj(ho_Cross, hv_WindowHandle);
                disp_message(hv_WindowHandle, "2、远离圆心，画箭头确定边缘检测方向，点击右键确认", "window", 12, 12, "red", "false");
                HOperatorSet.DrawLine(hv_WindowHandle, out hv_Row, out hv_Column, out hv_Row2, out hv_Column2);
                HOperatorSet.DistancePp(hv_RowC, hv_ColumnC, hv_Row, hv_Column, out hv_DistanceStart);
                HOperatorSet.DistancePp(hv_RowC, hv_ColumnC, hv_Row2, hv_Column2, out hv_DistanceEnd);
                HOperatorSet.LengthXld(ho_ContCircle, out hv_Length2);
                HOperatorSet.TupleLength(hv_ColXLD, out hv_Length3);
                if (new HTuple(hv_Elements.TupleLess(1)) != 0)
                {
                    hv_ROIRows = new HTuple();
                    hv_ROICols = new HTuple();
                    ho_ContOut.Dispose();
                    ho_Contour.Dispose();
                    ho_ContCircle.Dispose();
                    ho_Cross.Dispose();
                    ho_Rectangle.Dispose();
                    ho_Arrow.Dispose();
                }
                else
                {
                    HTuple end_val37 = hv_Elements - 1;
                    HTuple step_val37 = 1;
                    HTuple hv_i = 0;
                    while (hv_i.Continue(end_val37, step_val37))
                    {
                        if (new HTuple(hv_RowXLD.TupleSelect(0).TupleEqual(hv_RowXLD.TupleSelect(hv_Length3 - 1))) != 0)
                        {
                            HOperatorSet.TupleInt(1.0 * hv_Length3 / (hv_Elements - 1) * hv_i, out hv_j);
                        }
                        else
                        {
                            HOperatorSet.TupleInt(1.0 * hv_Length3 / (hv_Elements - 1) * hv_i, out hv_j);
                        }
                        if (new HTuple(hv_j.TupleGreaterEqual(hv_Length3)) != 0)
                        {
                            hv_j = hv_Length3 - 1;
                        }
                        hv_RowE = hv_RowXLD.TupleSelect(hv_j);
                        hv_ColE = hv_ColXLD.TupleSelect(hv_j);
                        if (new HTuple(hv_DistanceStart.TupleGreater(hv_DistanceEnd)) != 0)
                        {
                            HOperatorSet.TupleAtan2(-hv_RowE + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                            hv_ATan = new HTuple(180).TupleRad() + hv_ATan;
                            hv_Direct = "inner";
                        }
                        else
                        {
                            HOperatorSet.TupleAtan2(-hv_RowE + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                            hv_Direct = "outer";
                        }
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowE, hv_ColE, hv_ATan, hv_DetectHeight / 2, hv_DetectWidth / 2);
                        HOperatorSet.ConcatObj(ho_Regions, ho_Rectangle, out ExpTmpOutVar_0);
                        ho_Regions.Dispose();
                        ho_Regions = ExpTmpOutVar_0;
                        if (new HTuple(hv_i.TupleEqual(0)) != 0)
                        {
                            hv_RowL2 = hv_RowE + hv_DetectHeight / 2 * (-hv_ATan).TupleSin();
                            hv_RowL3 = hv_RowE - hv_DetectHeight / 2 * (-hv_ATan).TupleSin();
                            hv_ColL2 = hv_ColE + hv_DetectHeight / 2 * (-hv_ATan).TupleCos();
                            hv_ColL3 = hv_ColE - hv_DetectHeight / 2 * (-hv_ATan).TupleCos();
                            ho_Arrow.Dispose();
                            gen_arrow_contour_xld(out ho_Arrow, hv_RowL3, hv_ColL3, hv_RowL2, hv_ColL2, 25, 25);
                            HOperatorSet.ConcatObj(ho_Regions, ho_Arrow, out ExpTmpOutVar_0);
                            ho_Regions.Dispose();
                            ho_Regions = ExpTmpOutVar_0;
                        }
                        hv_i = hv_i.TupleAdd(step_val37);
                    }
                    ho_ContOut.Dispose();
                    ho_Contour.Dispose();
                    ho_ContCircle.Dispose();
                    ho_Cross.Dispose();
                    ho_Rectangle.Dispose();
                    ho_Arrow.Dispose();
                }
            }
        }
        public static void select_max_length_contour(HObject ho_Contours, out HObject ho_MaxLengthContour)
        {
            HObject ho_ObjectSelected = null;
            HTuple hv_Number = new HTuple();
            HTuple hv_Exception = null;
            HTuple hv_Length = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_MaxLengthContour);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            ho_MaxLengthContour.Dispose();
            HOperatorSet.GenEmptyObj(out ho_MaxLengthContour);
            try
            {
                HOperatorSet.CountObj(ho_Contours, out hv_Number);
            }
            catch (HalconException HDevExpDefaultException)
            {
                HDevExpDefaultException.ToHTuple(out hv_Exception);
                ho_ObjectSelected.Dispose();
                return;
            }
            if (new HTuple(hv_Number.TupleLess(1)) != 0)
            {
                ho_ObjectSelected.Dispose();
            }
            else
            {
                HTuple hv_Max_Length = 0;
                HTuple hv_Max_Index = 0;
                HTuple end_val21 = hv_Number;
                HTuple step_val21 = 1;
                HTuple hv_i = 1;
                while (hv_i.Continue(end_val21, step_val21))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_Contours, out ho_ObjectSelected, hv_i);
                    HOperatorSet.LengthXld(ho_ObjectSelected, out hv_Length);
                    if (new HTuple(hv_Max_Length.TupleLess(hv_Length)) != 0)
                    {
                        hv_Max_Length = hv_Length.Clone();
                        hv_Max_Index = hv_i.Clone();
                    }
                    hv_i = hv_i.TupleAdd(step_val21);
                }
                ho_MaxLengthContour.Dispose();
                HOperatorSet.SelectObj(ho_Contours, out ho_MaxLengthContour, hv_Max_Index);
                ho_ObjectSelected.Dispose();
            }
        }
        public static void dev_display_measure_object(HTuple hv_WindowHandle, HTuple hv_Row, HTuple hv_Col, HTuple hv_Phi, HTuple hv_Length1, HTuple hv_Length2)
        {
            HObject ho_Contour;
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HTuple hv_RowStart = hv_Row + hv_Phi.TupleSin() * hv_Length1;
            HTuple hv_RowEnd = hv_Row - hv_Phi.TupleSin() * hv_Length1;
            HTuple hv_ColStart = hv_Col - hv_Phi.TupleCos() * hv_Length1;
            HTuple hv_ColEnd = hv_Col + hv_Phi.TupleCos() * hv_Length1;
            HTuple hv_drow = (new HTuple(90).TupleRad() - hv_Phi).TupleSin() * hv_Length2;
            HTuple hv_dcol = (new HTuple(90).TupleRad() - hv_Phi).TupleCos() * hv_Length2;
            ho_Contour.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_Contour, (hv_RowStart - hv_drow).TupleConcat(hv_RowEnd - hv_drow).TupleConcat(hv_RowEnd + hv_drow).TupleConcat(hv_RowStart + hv_drow).TupleConcat(hv_RowStart - hv_drow), (hv_ColStart - hv_dcol).TupleConcat(hv_ColEnd - hv_dcol).TupleConcat(hv_ColEnd + hv_dcol).TupleConcat(hv_ColStart + hv_dcol).TupleConcat(hv_ColStart - hv_dcol));
            HOperatorSet.SetLineWidth(hv_WindowHandle, 2);
            HOperatorSet.SetColor(hv_WindowHandle, "blue");
            HOperatorSet.DispObj(ho_Contour, hv_WindowHandle);
            HOperatorSet_Ex.dev_display_profile_line(hv_WindowHandle, hv_Row, hv_Col, hv_Phi, hv_Length1, hv_Length2);
            ho_Contour.Dispose();
        }
        public static string ReadIniString(string section, string key, string noText, string iniFilePath)
        {
            StringBuilder temp = new StringBuilder(1024);
            InitFile.ReadString(section, key, noText, temp, 1024, iniFilePath);
            return temp.ToString();
        }
        public static bool Read_SetParm(string path, string RoiType, out string strPram, out HTuple hv_Rows, out HTuple hv_Cols, out HTuple hv_Direct)
        {
            strPram = null;
            hv_Rows = new HTuple();
            hv_Cols = new HTuple();
            hv_Direct = new HTuple();
            try
            {
                path = path + "\\" + RoiType;

                if (Directory.Exists(path))
                {
                    strPram = ReadIniString("Parm", "卡尺数量：", "", path + @"\Parm.ini");
                    strPram += ",";
                    strPram += ReadIniString("Parm", "卡尺长度：", "", path + @"\Parm.ini");
                    strPram += ",";
                    strPram += ReadIniString("Parm", "卡尺宽度：", "", path + @"\Parm.ini");
                    strPram += ",";
                    strPram += ReadIniString("Parm", "边缘阀值：", "", path + @"\Parm.ini");
                    strPram += ",";
                    strPram += ReadIniString("Parm", "平滑系数：", "", path + @"\Parm.ini");
                    strPram += ",";
                    strPram += ReadIniString("Parm", "极性：", "", path + @"\Parm.ini");
                    strPram += ",";
                    strPram += ReadIniString("Parm", "点选择：", "", path + @"\Parm.ini");
                }
                HOperatorSet.ReadTuple(path  + @"\hv_Rows.tup", out hv_Rows);
                HOperatorSet.ReadTuple(path  + @"\hv_Cols.tup", out hv_Cols);
                if (RoiType.Substring(0, 4) == "Circ")
                    HOperatorSet.ReadTuple(path +  @"\hv_Direct.tup", out hv_Direct);
                return true;
            }
            catch (HalconException)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(RoiType + " Parameter loading failed!", "Warning");
                }
                else
                {
                    MessageBox.Show(RoiType + "参数加载失败！", "警告！");
                }
                
                return false;
            }
        }
        public static void Fit_Line( HObject m_image, HTuple h_WindowHandle, string strPram, HTuple  hv_Rows, HTuple hv_Cols, out HTuple hv_RowBegin, out HTuple hv_ColBegin, out HTuple hv_RowEnd, out HTuple hv_ColEnd)
        {
            HTuple timebegin = null;
            HOperatorSet.CountSeconds(out timebegin);
            hv_RowBegin =new HTuple();
            hv_ColBegin= new HTuple();
            hv_RowEnd = new HTuple();
            hv_ColEnd = new HTuple();
            HObject ho_spoke = null, ho_circle = null;
            //Read_SetParm(path+"\\"+ RoiType, RoiType, out strPram, out hv_Rows, out hv_Cols, out hv_Direct);
            string[] str = strPram.Split(',');
            string C_Rule = str[0];
            string C_RuleHeiht = str[1];
            string C_RuleWith = str[2];
            string C_Smooth = str[3];
            string C_Threshold = str[4];
            string C_Polarity = str[5];
            string C_PointSelect = str[6];
            bool b_txt = (C_Rule != "" && C_RuleHeiht != "" &&
                          C_RuleWith != "" && C_Smooth != "" &&
                          C_Threshold != "" && C_Polarity != "" && C_PointSelect != "");
            if (b_txt != true)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Setting 1 parameter loading failed!", "Warning");
                }
                else
                {
                    MessageBox.Show("设定1参数加载失败！", "警告！");
                }
                
                return;
            }

            if (m_image != null)
            {
                if (ho_spoke != null)
                {
                    ho_spoke.Dispose();//释放内存
                }
                if (ho_circle != null)
                {
                    ho_circle.Dispose();//释放内存
                }
                string str_polarity = Polarity(C_Polarity);
                string str_pointSelect = PointSelect(C_PointSelect);
                HTuple Rowc, hv_circle_ResultRow, hv_circle_ResultCol;
                HTuple Colc;
                HTuple Lenght, phi;
                HOperatorSet.LinePosition(hv_Rows.TupleSelect(0), hv_Cols.TupleSelect(0), hv_Rows.TupleSelect(1), hv_Cols.TupleSelect(1), out Rowc, out Colc, out Lenght, out phi);
                HOperatorSet_Ex.rake(m_image, out ho_spoke,
                    int.Parse(C_Rule),
                    int.Parse(C_RuleHeiht),
                    int.Parse(C_RuleWith),
                    double.Parse(C_Smooth),
                    double.Parse(C_Threshold),
                    str_polarity,
                    str_pointSelect,
                    hv_Rows.TupleSelect(0), hv_Cols.TupleSelect(0), hv_Rows.TupleSelect(1), hv_Cols.TupleSelect(1), out hv_circle_ResultRow, out hv_circle_ResultCol);
                HOperatorSet.SetColored(h_WindowHandle, 12);
                HOperatorSet.DispObj(ho_spoke, h_WindowHandle);
                if (hv_circle_ResultRow.TupleLength() < (int.Parse(C_Rule)) / 2)//失败的点超过一半
                {
                    return;
                }
                pts_to_best_line(out ho_circle, hv_circle_ResultRow, hv_circle_ResultCol, (int.Parse(C_Rule)) / 2, out hv_RowBegin, out hv_ColBegin, out hv_RowEnd, out hv_ColEnd);
                HOperatorSet.SetColor(h_WindowHandle, "green");
                double Cross = double.Parse(C_RuleWith);
                Disp_Coss(h_WindowHandle, hv_circle_ResultRow, hv_circle_ResultCol, Cross / 2, new HTuple(45).TupleRad() + phi);

                HOperatorSet.DispObj(ho_circle, h_WindowHandle);
                HOperatorSet.SetColor(h_WindowHandle, "red");
                //set_display_font(h_WindowHandle, 10, "mono", "true", "false");// '字体大小设置;
                HTuple timeEnd = null;
                HOperatorSet.CountSeconds(out timeEnd);
                //disp_message(h_WindowHandle,
                //           "直线点1:" + (hv_RowBegin.TupleString(".3f")) + "," + (hv_ColBegin.TupleString(".3f")) + "\n" +
                //           "直线点2:" + (hv_RowEnd.TupleString(".3f")) + "," + (hv_ColEnd.TupleString(".3f")) + "\n"+
                //           "耗时:" + ((timeEnd - timebegin).TupleString(".3f")) + "秒\n",
                //           "Window", 10, 10, "black", "true");
            }

            else
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Please load the picture!", "Warning！");
                }
                else
                {
                    MessageBox.Show("请加载图片！", "警告！");
                }
            }
        }
        public static string Polarity(string Polarity)
        {
            string polarity = "";
            if (Polarity == "所有")
            { polarity = "all"; }
            else if (Polarity == "黑到白")
            { polarity = "positive"; }
            else if (Polarity == "白到黑")
            { polarity = "negative"; }
            return polarity;
        }
        public static string PointSelect(string PointSelect)
        {

            string pointSelect = "";
            if (PointSelect == "所有点")
            { pointSelect = "all"; }
            else if (PointSelect == "第一点")
            { pointSelect = "first"; }
            else if (PointSelect == "最后点")
            { pointSelect = "last"; }
            return pointSelect;
        }
        public static void Disp_Coss(HTuple hv_WindowHandle, HTuple hv_Row, HTuple hv_Col, HTuple hv_Size, HTuple hv_angle)
        {
            HObject ho_cross;
            HOperatorSet.GenCrossContourXld(out ho_cross, hv_Row, hv_Col, hv_Size, hv_angle);
            HOperatorSet.DispObj(ho_cross, hv_WindowHandle);
        }
        public static void Fit_Circle( HObject m_image, HTuple h_WindowHandle, string strPram, HTuple hv_Rows, HTuple hv_Cols, HTuple hv_Direct, out HTuple hv_RowCenter, out HTuple hv_ColCenter, out HTuple hv_Radius)
        {
            HTuple timebegin = null; 
            HOperatorSet.CountSeconds(out timebegin);
            hv_RowCenter = new HTuple();
            hv_ColCenter = new HTuple();
            hv_Radius = new HTuple();

            HObject ho_spoke = null, ho_circle = null;
            //Read_SetParm(path + "\\" + RoiType, RoiType, out strPram, out hv_Rows, out hv_Cols, out hv_Direct);
            string[] str = strPram.Split(',');
            string C_Rule = str[0];
            string C_RuleHeiht = str[1];
            string C_RuleWith = str[2];
            string C_Smooth = str[3];
            string C_Threshold = str[4];
            string C_Polarity = str[5];
            string C_PointSelect = str[6];
            bool b_txt = (C_Rule != "" && C_RuleHeiht != "" &&
                          C_RuleWith != "" && C_Smooth != "" &&
                          C_Threshold != "" && C_Polarity != "" && C_PointSelect != "");
            if (b_txt != true)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Setting 1 parameter loading failed!", "Warning");
                }
                else
                {
                    MessageBox.Show("设定1参数加载失败！", "警告！");
                }
                return;
            }

            if (m_image != null)
            {
                if (ho_spoke != null)
                {
                    ho_spoke.Dispose();//释放内存
                }
                if (ho_circle != null)
                {
                    ho_circle.Dispose();//释放内存
                }
                string str_polarity = Polarity(C_Polarity);
                string str_pointSelect = PointSelect(C_PointSelect);
                HTuple hv_circle_ResultRow, hv_circle_ResultCol, hv_spokeArcType;

                HOperatorSet_Ex.spoke(m_image, out ho_spoke,
                int.Parse(C_Rule),
                int.Parse(C_RuleHeiht),
                int.Parse(C_RuleWith),
                double.Parse(C_Smooth),
                double.Parse(C_Threshold),
                str_polarity,
                str_pointSelect,
                hv_Rows, hv_Cols, hv_Direct, out hv_circle_ResultRow, out hv_circle_ResultCol, out hv_spokeArcType);
                HOperatorSet.SetColored(h_WindowHandle, 12);
                HOperatorSet.DispObj(ho_spoke, h_WindowHandle);
                if (hv_circle_ResultRow.TupleLength() < (int.Parse(C_Rule)) / 2)//失败的点超过一半
                {
                    return;
                }
                pts_to_best_circle(out ho_circle, hv_circle_ResultRow, hv_circle_ResultCol, (int.Parse(C_Rule)) / 2, hv_spokeArcType, out hv_RowCenter, out hv_ColCenter, out hv_Radius);
                HOperatorSet.SetColor(h_WindowHandle, "green");
                double Cross = double.Parse(C_RuleWith);
                Disp_Coss(h_WindowHandle, hv_circle_ResultRow, hv_circle_ResultCol, Cross / 2, new HTuple(45).TupleRad());
                Disp_Coss(h_WindowHandle, hv_RowCenter, hv_ColCenter, Cross * 2, 0);

                HOperatorSet.SetColor(h_WindowHandle, "red");
                HOperatorSet.DispObj(ho_circle, h_WindowHandle);
                //set_display_font(h_WindowHandle, 10, "mono", "true", "false");// '字体大小设置;
                HTuple timeEnd = null;
                HOperatorSet.CountSeconds(out timeEnd);
                //disp_message(h_WindowHandle, "圆的半径:" + (hv_Radius.TupleString(".3f")) + "\n" +
                //           "圆心RowCenter:" + (hv_RowCenter.TupleString(".3f")) + "\n" +
                //           "圆心ColCenter:" + (hv_ColCenter.TupleString(".3f")) + "\n"+
                //           "耗时:" + ((timeEnd-timebegin).TupleString(".3f")) + "秒\n",
                //           "Window", 10, 10, "black", "true");
            }
            else
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Please load the picture!", "Warning！");
                }
                else
                {
                    MessageBox.Show("请加载图片！", "警告！");
                }
            }
        }

    }
}

