Imports System
Imports System.Resources
Imports System.Globalization
Imports System.Threading
Imports System.Reflection
Imports System.Data.SqlClient
Imports SalesInterface.MobileSales

'Imports System.Data.Odbc

Public Class ImportStatus
    Implements ISalesBase

    Private aAgent As New ArrayList()
    Dim arRem As New ArrayList
    Dim strPay As String = " "
    Dim strDate As String = " "

    Dim bDeleteIns As Boolean = False
    Dim dImpStartDate As Date = Date.Now

    Private sNavCompanyName As String = ""
    Private arrOrdNo As New ArrayList()
    Private arrRecNo As New ArrayList()
    Private arrCrNo As New ArrayList()
    Private arrNewCustNo As New ArrayList()
    Public sCustPostGroup, sGenPostGroup, sGSTPostGroup, sGSTProdGroup, sGenJournalTemplate, sGenJournalBatch, sWorkSheetTemplate, sJournalBatch, sItemJnlTemplate, sItemJnlBatch, sFocBatch, sExBatch, sBadLoc, sItemReclassTemplate, sItemReclassBatch As String
    Private Structure DelCust
        Dim CustID As String
        Dim PrGroup As String
    End Structure
    Dim i, igCnt As Integer
    'Dim cnt As Integer = 0
    'Private NavCompanyName As String = GetCompanyName()
    Private Structure ArrRemarks
        Dim sOrdNo As String
        Dim sRemarks As String
    End Structure
    Private Structure ArrItemPrice
        Dim ItemCode As String
        Dim SalesCode As String
        Dim MaxDate As Date
        Dim SalesType As Integer
        Dim MinQty As Double
        Dim VariantCode As String
        Dim sUOM As String
    End Structure
    Private Structure ArrPurchaseOrder
        Dim PONo As String
        Dim PODt As Date
        Dim VendorId As String
        Dim AgentID As String
        Dim Discount As String
        Dim SubTotal As String
        Dim GSTAmt As String
        Dim TotalAmt As String
        Dim Void As String
        Dim PrintNo As String
        Dim PayTerms As String
        Dim CurCode As String
        Dim CurExRate As String
        Dim Exported As String
        Dim DTG As Date
        Dim DeliveryDate As Date
        Dim LocationCode As String
        Dim ExternalDocNo As String
        Dim Remarks As String
        Dim POType As String
        Dim ContainerNo As String
        Dim Department As String
        Dim ManufacturerCode As String
        Dim CompanyName As String
        Dim DocEntry As String
    End Structure
    Private Structure ArrDiscGroup
        Dim ItemCode As String
        Dim SalesCode As String
        Dim MaxDate As Date
        Dim SalesType As String
        Dim MinQty As Double
        Dim VariantCode As String
        Dim sUOM As String
    End Structure
    Private Structure ArrInvoice
        Dim InvNo As String
        Dim InvDate As Date
        Dim OrdNo As String
        Dim CustID As String
        Dim AgentID As String
        Dim Discount As Double
        Dim Subtotal As Double
        Dim GST As Double
        Dim GSTAmt As Double
        Dim TotalAmt As Double
        Dim PaidAmt As Double
        Dim payterms As String
        Dim TermDays As String
        Dim CurExRate As String
        Dim CurCode As String
    End Structure

    Private Structure ArrCrNote
        Dim CrNo As String
        Dim CrDate As Date
        Dim OrdNo As String
        Dim CustID As String
        Dim AgentID As String
        Dim Discount As Double
        Dim Subtotal As Double
        Dim GST As Double
        Dim GSTAmt As Double
        Dim TotalAmt As Double
        Dim PaidAmt As Double
        Dim payterms As String
        Dim TermDays As String
        Dim CurExRate As String
        Dim CurCode As String
    End Structure
    Private Structure ArrPrCrNote
        Dim DocEntry As String
        Dim CreditNoteNo As String
        Dim ItemNo As String
        Dim UOM As String
        Dim Qty As String
        Dim Price As String
        Dim Amt As String
        Dim VariantCode As String
        Dim Description As String
        Dim FromLocation As String
        Dim FromBin As String
        Dim DeliQty As String
        Dim Remarks As String
      
    End Structure

    Private Structure Arrord
        Dim Ordno As String
        Dim DocEntry As String
    End Structure
    Private Structure ArrTrans
        Dim transno As String
        Dim DocEntry As String
    End Structure
    Private Sub ImportStatus_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        DisconnectDB()
    End Sub
    Private Function GetCompanyName() As String
        Dim ds As New DataSet
        Dim dataDirectory As String
        dataDirectory = Windows.Forms.Application.StartupPath
        ds.ReadXml(dataDirectory & "\Simplr.xml")
        Dim table As DataTable
        For Each table In ds.Tables
            Dim row As DataRow
            If table.TableName = "CompanyName" Then
                For Each row In table.Rows
                    'MsgBox(row("CompanyName").ToString())
                    Return row("Value").ToString()
                Next row
            End If
        Next table
        Return ""
    End Function

    Private Sub ImportStatus_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave

    End Sub

    Private Sub ImportStatus_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ConnectDB()
        Dim ArrList As New ArrayList
        '  cnt = 0
        ArrList.Add("Customer")
        ArrList.Add("Category")
        ArrList.Add("Product")
        ArrList.Add("Item Price")
        ArrList.Add("Location")
        ArrList.Add("Agent")
        ArrList.Add("Payment Terms")
        ArrList.Add("Invoice")
        ArrList.Add("CreditNote")
        ArrList.Add("UOM")
        ArrList.Add("Inventory")
        ArrList.Add("Vendor")
        ArrList.Add("PurchaseOrder")
        ArrList.Add("Purchasecreditnote")
        ArrList.Add("Salesorder")
        ArrList.Add("TransferOrder")
        dgvStatus.Rows.Clear()
        Dim dtr As SqlDataReader
        For i = 0 To ArrList.Count - 1
            dtr = ReadRecord("Select Status from ImportStatus where TableName = " & SafeSQL(ArrList.Item(i)))
            If dtr.Read = True Then
                dgvStatus.Rows.Add(ArrList.Item(i).ToString, CBool(dtr("Status")))
            Else
                dgvStatus.Rows.Add(ArrList.Item(i).ToString, True)
            End If
            dtr.Close()
        Next
        'loadCombo()
        ' LoadExportCombo()
        chkOrders.Checked = True
        btnIm_Click(Me, Nothing)


        'Try

        '    ExecuteSQL("Insert into GoodsInvnDailyClosing (Location, ItemNo, UOM, Qty, BinNo, DTG) Select Location, ItemNo, UOM, Qty, BinNo, GetDate() from GoodsInvn where Qty <>0")

        'Catch ex As Exception

        'End Try


        Me.Close()

    End Sub

    Public Function GetListViewForm() As String Implements SalesInterface.MobileSales.ISalesBase.GetListViewForm
        Return ""
    End Function

    Public Sub ListViewClick() Implements SalesInterface.MobileSales.ISalesBase.ListViewClick

    End Sub

    Public Event LoadDataForm(ByVal Location As String, ByVal LoadType As String, ByVal ParentLoadType As String, ByVal ResultTo As String, ByVal XPos As Integer, ByVal YPos As Integer) Implements SalesInterface.MobileSales.ISalesBase.LoadDataForm

    Public Event LoadForm(ByVal Location As String, ByVal LoadType As String) Implements SalesInterface.MobileSales.ISalesBase.LoadForm

    Public Sub MoveFirstClick() Implements SalesInterface.MobileSales.ISalesBase.MoveFirstClick

    End Sub

    Public Sub MoveLastClick() Implements SalesInterface.MobileSales.ISalesBase.MoveLastClick

    End Sub

    Public Sub MoveNextClick() Implements SalesInterface.MobileSales.ISalesBase.MoveNextClick

    End Sub

    Public Sub MovePositionClick(ByVal Position As Long) Implements SalesInterface.MobileSales.ISalesBase.MovePositionClick

    End Sub

    Public Sub MovePreviousClick() Implements SalesInterface.MobileSales.ISalesBase.MovePreviousClick

    End Sub

    Public Sub Print() Implements SalesInterface.MobileSales.ISalesBase.Print

    End Sub

    Public Sub Print(ByVal PageNo As Integer) Implements SalesInterface.MobileSales.ISalesBase.Print

    End Sub

    Public Event ResultData(ByVal ChildLoadType As String, ByVal Value As String) Implements SalesInterface.MobileSales.ISalesBase.ResultData

    Public Sub ReturnedData(ByVal Value As String, ByVal ResultTo As String, ByVal XPos As Integer, ByVal YPos As Integer) Implements SalesInterface.MobileSales.ISalesBase.ReturnedData

    End Sub

    Public Sub ReturnSearch(ByVal SQL As String) Implements SalesInterface.MobileSales.ISalesBase.ReturnSearch

    End Sub

    Public Event SearchField(ByVal FormName As String, ByVal FieldName As String, ByVal FieldType As String, ByVal CurValue As String) Implements SalesInterface.MobileSales.ISalesBase.SearchField

    Public Sub SetCulture(ByVal CultureName As String) Implements SalesInterface.MobileSales.ISalesBase.SetCulture

    End Sub


    Private Function GetLastImpDate() As Date
        Dim dDate As Date = Date.Now
        Dim dtr As SqlDataReader
        dtr = ReadRecord("Select LastImExDate from System")
        If dtr.Read Then
            dDate = dtr("LastImExDate")
            'bFound = True
        End If
        dtr.Close()
        dtr.Dispose()
        Return dDate
    End Function

    Private Sub btnIm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIm.Click
        Try
            btnIm.Enabled = False
            btnEx.Enabled = False
            btnDelete.Enabled = False
            Dim frmStatus As New status
            frmStatus.Show()
            sNavCompanyName = GetCompanyName()
            System.IO.File.AppendAllText("C:\ImportLog.txt", " " & vbCrLf)
            System.IO.File.AppendAllText("C:\ImportLog.txt", "Start Import: " & Date.Now.ToString)
            ExecuteSQL("Update System set Status='Not Completed'")
            ConnectNavDB()
            ConnectAnotherDB()

            Dim LastImpDate As Date = GetLastImpDate()
            ExecuteSQL("Update System set LastImExDate = " & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")))

            If Format(LastImpDate, "dd") <> Format(dImpStartDate, "dd") Then
                bDeleteIns = True
            End If


            If bDeleteIns = True Then
                Try
                    ExecuteSQLAnother("Delete from ErrorLog where DTG < GetDate()-30")
                    ExecuteSQLAnother("Insert into ExpiryItemDailyClosing (Location, ItemNo, UOM, Qty, LotNo, ExpiryDate, BinNo, SerialNo, PalletNo, ReceivedDate, DTG) Select Location, ItemNo, UOM, Qty, LotNo, ExpiryDate, BinNo, SerialNo, PalletNo, ReceivedDate, GetDate() from ExpiryItem, Location where ExpiryItem.Location=Location.Code and Location.ShowInInventory=1 and Qty <>0")
                    ExecuteSQLAnother("Update ExpiryItem Set Qty = 0 where Qty < 0.001 and Qty > -0.001 and Qty <> 0")
                    ExecuteSQLAnother("Update GoodsInvn Set Qty = 0 where Qty < 0.001 and Qty > -0.001 and Qty <> 0")
                    ExecuteSQLAnother("Delete from ExpiryItem where Qty=0")
                    ExecuteSQLAnother("Delete from GoodsInvn where Qty=0")
                Catch ex As Exception

                End Try
            End If


            For igCnt = 0 To dgvStatus.Rows.Count - 1
                If dgvStatus.Rows(igCnt).IsNewRow = True Then Exit For
                If dgvStatus.Item(1, igCnt).Value = True Then
                    Select Case dgvStatus.Item(0, igCnt).Value.ToString
                        Case "Customer"
                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - Customer'," & SafeSQL("") & "," & SafeSQL("") & ")")
                            'ImportDimension()
                            ImportCustomer()
                            ImportCustGroup()
                            'ImportShipToAddress()
                            ' ImportZone()
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "Category"
                            If bDeleteIns = True Then
                                ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - Category'," & SafeSQL("") & "," & SafeSQL("") & ")")
                                ImportCategory()
                                ImportBrand()
                            End If
                            '  ImportSubCategory()
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "Product"
                            ' ImportItemVariant()
                            If bDeleteIns = True Then
                                ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - Product'," & SafeSQL("") & "," & SafeSQL("") & ")")
                                ImportProduct()
                                ImportBarcodes()
                            End If


                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "Item Price"
                            'ImportItemPrice()
                            '      ImportItemPricePromotion()
                            '     ImportCustDiscGroup()
                            '    ImportCustDiscPromotion()
                            '    ImportCustInvDiscount()
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Item Price'")
                            Me.Refresh()
                            'MsgBox(igCnt)
                            'MsgBox(i & " : " & dgvStatus.Item(0, i).Value)
                            dgvStatus.Item(1, igCnt).Value = False
                            'MsgBox("Price")
                        Case "Location"
                            If bDeleteIns = True Then
                                ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - Location'," & SafeSQL("") & "," & SafeSQL("") & ")")
                                ImportLocation()
                            End If
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Location'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "Agent"
                            If bDeleteIns = True Then
                                ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - Agent'," & SafeSQL("") & "," & SafeSQL("") & ")")
                                ImportSalesAgent()
                            End If
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Agent'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "Payment Terms"
                            'ImportPayMethod()
                            If bDeleteIns = True Then
                                ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - Payment Term'," & SafeSQL("") & "," & SafeSQL("") & ")")
                                ImportPayterms()
                                ImportPriceGroup()
                            End If
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Payment Terms'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "Invoice"
                            'ImportInvoice()
                            'ImportCustProd()
                            '  ImportGenJournal()
                            '  UpdatePDCPaidAmt()
                            '  ImportOpeningBalance()
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Invoice'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()

                        Case "CreditNote"
                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - CreditNote'," & SafeSQL("") & "," & SafeSQL("") & ")")
                            ImportCreditNote()
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='CreditNote'")
                            dgvStatus.Item(1, igCnt).Value = False
                            '    MsgBox("CrNote")
                            Me.Refresh()

                        Case "UOM"
                            '   ImportUOM()
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='UOM'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "Inventory"
                            ' ImportInventory()
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Inventory'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()

                        Case "Vendor"
                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - Vendor'," & SafeSQL("") & "," & SafeSQL("") & ")")
                            ImportVendor()
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='vendor'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "PurchaseOrder"
                            '                            System.IO.File.AppendAllText("C:\ImportLog.txt", "     Start PO : " & Date.Now.ToString & vbCrLf)
                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - PO Start'," & SafeSQL("") & "," & SafeSQL("") & ")")
                            ImportPurchaseOrder()
                            'System.IO.File.AppendAllText("C:\ImportLog.txt", "     Finish PO: " & Date.Now.ToString & vbCrLf)
                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - PO End'," & SafeSQL("") & "," & SafeSQL("") & ")")

                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='PurchaseOrder'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "Purchasecreditnote"
                            '  ImportPurchasecreditnote()
                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Purchasecreditnote'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "Salesorder"
                            ' System.IO.File.AppendAllText("C:\ImportLog.txt", "     Start SO: " & Date.Now.ToString & vbCrLf)

                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - SO Start'," & SafeSQL("") & "," & SafeSQL("") & ")")

                            ImportSalesorder()
                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - SO Finish'," & SafeSQL("") & "," & SafeSQL("") & ")")

                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - DO Start'," & SafeSQL("") & "," & SafeSQL("") & ")")
                            ImportDeliveryorder()
                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - DO Finish'," & SafeSQL("") & "," & SafeSQL("") & ")")

                            ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Salesorder'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                        Case "TransferOrder"
                            ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Import - TransferOrder'," & SafeSQL("") & "," & SafeSQL("") & ")")
                            ImportTransferOrder()
                            ExecuteSQL("Update ImportStatus Set Status =0 where TableNAme = 'TransferOrder'")
                            dgvStatus.Item(1, igCnt).Value = False
                            Me.Refresh()
                    End Select
                End If
                frmStatus.Refresh()
            Next
            ExecuteSQL("Update System set Status='Completed'")
            frmStatus.Close()
            'MsgBox("Import Completed")
            DisConnect()

            System.IO.File.AppendAllText("C:\ImportLog.txt", "     Finish Import: " & Date.Now.ToString & vbCrLf)
        Catch ex As Exception
            btnIm.Enabled = True
            btnEx.Enabled = True
            btnDelete.Enabled = True
            System.IO.File.AppendAllText("C:\ErrorLog.txt", Date.Now & vbCrLf & ex.Message & vbCrLf)
            DisConnect()
        End Try
    End Sub

    Public Sub DisConnect()
        DisconnectNavDB()
        DisconnectAnotherDB()
        btnIm.Enabled = True
        btnEx.Enabled = True
        btnDelete.Enabled = True
        '    ExecuteSQL("Update System set LastImExDate = " & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")))
    End Sub



    Public Sub ImportCustProd()
        Dim dtr As SqlDataReader
        ExecuteSQL("Delete from CustProd")
        dtr = ReadRecord("SELECT Distinct Invoice.CustId, InvItem.ItemNo, Item.Description, Item.ItemName, Item.BaseUOM as UOM, Item.UnitPrice, Customer.PriceGroup FROM Invoice, InvItem, Item, Customer where Invoice.InvNo = InvItem.InvNo and InvItem.ItemNo = Item.ItemNo and Invoice.CustID=Customer.CustNo and (DateDiff(d, InvDt, " & SafeSQL(Format(DateAdd(DateInterval.Month, (-1) * 3, Date.Now.Date), "yyyyMMdd HH:mm:ss")) & ") <= 0)")
        'dtr = ReadRecord("Select Distinct Invoice.CustID, Customer.PriceGroup, InvItem.ItemNo, ShortDesc, Item.BaseUOM as UOM, Item.ItemName from InvItem, Invoice, Item, Customer where Customer.CustNo = Invoice.CustId and Invoice.InvNo = InvItem.InvNo and InvItem.ItemNo = Item.ItemNo")

        While dtr.Read
            Dim dPr As Double = 0
            Dim dItemPr As Double
            dItemPr = GetPrice(dtr("ItemNo").ToString, dtr("CustId").ToString, dtr("PriceGroup").ToString, dtr("UOM").ToString)
            'If dtr("UOM").ToString = "CTN" Then
            '    If IsCustProdExists(dtr("CustId").ToString, dtr("ItemNo").ToString, dtr("UOM").ToString) = False Then
            '        ExecuteSQLAnother("Insert into CustProd (CustId, ItemNo, Description, ItemName, Uom, Price) Values (" & SafeSQL(dtr("CustID")) & " , " & SafeSQL(dtr("ItemNo")) & ", " & SafeSQL(dtr("ShortDesc")) & "," & SafeSQL(dtr("ItemName").ToString) & ", " & SafeSQL(dtr("Uom")) & ", " & dItemPr & " )")
            '    End If
            'Else
            ExecuteSQLAnother("Insert into CustProd (CustId, ItemNo, Description, ItemName, Uom, Price) Values (" & SafeSQL(dtr("CustID")) & " , " & SafeSQL(dtr("ItemNo")) & ", " & SafeSQL(dtr("Description")) & "," & SafeSQL(dtr("ItemName").ToString) & ", " & SafeSQL(dtr("Uom")) & ", " & dItemPr & " )")
            'End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
    End Sub

    Public Function GetPrice(ByVal sItemId As String, ByVal sCustNo As String, ByVal sPrGroup As String, ByVal sUOM As String) As Double
        Dim dtr As SqlDataReader
        Dim dPr1 As Double = 0
        Dim dPr2 As Double = 0
        Dim dPr3 As Double = 0
        Dim dPr As Double = 0
        dtr = ReadRecordAnother("Select UnitPrice from ItemPr where UOM= " & SafeSQL(sUOM) & " and ItemNo = '" & sItemId & "' and PriceGroup = '" & sPrGroup & "' and SalesType = 'Customer Price Group' and MinQty=0")
        If dtr.Read Then
            dPr1 = dtr("UnitPrice")
            dPr = dPr1
        End If
        dtr.Close()
        dtr = ReadRecordAnother("Select UnitPrice from ItemPr where UOM= " & SafeSQL(sUOM) & " and  ItemNo = '" & sItemId & "' and PriceGroup = '" & sCustNo & "'and SalesType = 'Customer' and MinQty=0")
        If dtr.Read Then
            dPr2 = dtr("UnitPrice")
            If dPr2 < dPr And dPr2 > 0 Then
                dPr = dPr2
            End If
        End If
        dtr.Close()
        dtr = ReadRecordAnother("Select UnitPrice from ItemPr where UOM= " & SafeSQL(sUOM) & " and ItemNo = '" & sItemId & "' and SalesType = 'All Customers' and MinQty=0")
        If dtr.Read Then
            dPr3 = dtr("UnitPrice")
            If dPr3 < dPr And dPr3 > 0 Then
                dPr = dPr3
            End If
        End If
        dtr.Close()
        If dPr = 0 Then
            dtr = ReadRecordAnother("Select UnitPrice from Item where ItemNo = '" & sItemId & "'")
            If dtr.Read Then
                dPr = dtr("UnitPrice")
            End If
            dtr.Close()
        End If
        Return dPr
    End Function

    Private Function IsExists(ByVal strSql As String) As Boolean
        Dim dtr As SqlDataReader
        Dim bAns As Boolean
        dtr = ReadRecord(strSql)
        bAns = dtr.Read
        dtr.Close()
        Return bAns
    End Function

    Public Sub ImportShipToAddress()
        Dim dt As DateTime
        Dim dtr As SqlDataReader
        dt = Date.Now
        ExecuteSQL("Delete CustomerBill")
        ExecuteSQL("Update Customer Set BillMultiple = 0 ")
        Dim icnt As Integer = 1
        dtr = ReadNavRecord("Select * from """ & sNavCompanyName & "Ship-to Address""")
        While dtr.Read
            If IsExists("Select * from CustomerBill where AcBillRef = " & SafeSQL(dtr("Code").ToString) & " and CustNo = " & SafeSQL(dtr("Customer No_").ToString)) = True Then
                'Update
                ExecuteSQL("Update CustomerBill Set CustName = " & SafeSQL(dtr("Name").ToString) & ",Address = " & SafeSQL(dtr("Address").ToString) & " , Address2 = " & SafeSQL(dtr("Address 2").ToString) _
                        & " , City = " & SafeSQL(dtr("City").ToString) & ",ContactPerson = " & SafeSQL(dtr("Contact").ToString) & " ,Phone = " & SafeSQL(dtr("Phone No_").ToString) _
                        & " , PostCode = " & SafeSQL(dtr("Post Code").ToString) _
                        & " where AcBillRef = " & SafeSQL(dtr("Code").ToString) & " and CustNo = " & SafeSQL(dtr("Customer No_").ToString))
            Else
                'Insert
                ExecuteSQL("Insert into CustomerBill(AcBillRef, CompanyName, CustNo, CustName, Address, Address2, City, ContactPerson, Phone, PostCode) VALUES " _
                        & "(" & SafeSQL(dtr("Code").ToString) & "," & SafeSQL("STD") & "," & SafeSQL(dtr("Customer No_").ToString) & "," & SafeSQL(dtr("Name").ToString) _
                        & "," & SafeSQL(dtr("Address").ToString) & "," & SafeSQL(dtr("Address 2").ToString) & "," & SafeSQL(dtr("City").ToString) & "," & SafeSQL(dtr("Contact").ToString) _
                        & "," & SafeSQL(dtr("Phone No_").ToString) & "," & SafeSQL(dtr("Post Code").ToString) & ")")

            End If
            ExecuteSQL("Update Customer Set BillMultiple = 1 where CustNo = " & SafeSQL(dtr("Customer No_").ToString))
        End While
        dtr.Close()
    End Sub

    Private Sub UpdateLastTimeStamp(ByVal TableName As String, ByVal iValue As Date)
        ExecuteSQL("Update SyncTimeStamp Set LastTimeStamp=1111, LastImportDate = " & SafeSQL(Format(Date.Now, "yyyyMMdd")) & " where TableName = " & SafeSQL(TableName))
    End Sub

    Private Sub InsertLastTimeStamp(ByVal TableName As String, ByVal iValue As Date)
        ExecuteSQL("Insert into SyncTimeStamp (LastTimeStamp, TableName, LastImportDate) values (1111," & SafeSQL(TableName) & "," & SafeSQL(Format(Date.Now, "yyyyMMdd")) & ")")
    End Sub


    Private Function GetLastTimeStamp(ByVal TableName As String, ByRef iValue As Date, ByRef dNewRecord As Int16) As Boolean
        Dim bFound As Boolean = False
        Dim dtr As SqlDataReader
        dtr = ReadRecord("Select LastImportDate, LastTimeStamp from SyncTimeStamp where TableName = " & SafeSQL(TableName))
        If dtr.Read Then
            iValue = dtr("LastImportDate")
            dNewRecord = dtr("LastTimeStamp")
            bFound = True
        End If
        dtr.Close()
        Return bFound
    End Function

    Public Sub ImportCustomer()
        Dim dtr As SqlDataReader
        Dim icnt As Integer = 1
        Dim iValue As Date = Date.Now
        Dim dNewRecord As Int16 = 0
        Dim bSync As Boolean = GetLastTimeStamp("Customer", iValue, dNewRecord)
        If bDeleteIns = True Then
            dtr = ReadNavRecord("SELECT * FROM OCRD WHERE (CardType = 'C')")
        Else
            dtr = ReadNavRecord("SELECT * FROM OCRD WHERE (CardType = 'C') and (UpdateDate >= " & SafeSQL(Format(iValue, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        End If

        Dim sdCustNo As String = "", Str As String = ""
        While dtr.Read

            sdCustNo = dtr("CardCode").ToString
            If dtr("CardCode").ToString <> "" And IsCustomerExists(sdCustNo) = True Then
                Str = "Update Customer Set CustName = " & SafeSQL(dtr("CardName").ToString.Trim) & ", Address = " & SafeSQL(dtr("Address").ToString.Trim) & ", Address2 = " & SafeSQL("") & ", Address3 = '', Address4 = '', PostCode = " & SafeSQL(dtr("ZipCode").ToString.Trim) & ", City = '', CountryCode = " & SafeSQL(dtr("Country").ToString.Trim) & ", Phone = " & SafeSQL(dtr("Phone1").ToString.Trim) & ", ContactPerson =" & SafeSQL(dtr("CntctPrsn").ToString.Trim) & ", Balance = " & IIf(IsDBNull(dtr("Balance")), 0, dtr("Balance")) & ", CreditLimit = " & IIf(IsDBNull(dtr("CreditLine")), 0, dtr("CreditLine")) & ", FaxNo = " & SafeSQL(dtr("Fax").ToString.Trim) & ", email = " & SafeSQL(dtr("E_Mail").ToString.Trim) & ", " & _
                          "PriceGroup = " & SafeSQL(dtr("ListNum").ToString.Trim) & ", Category = " & SafeSQL(dtr("GroupCode").ToString.Trim) & ", SearchName = " & SafeSQL(dtr("CardName").ToString.Trim) & ", PaymentTerms = " & SafeSQL(dtr("GroupNum").ToString.Trim) & ", PaymentMethod = 'CASH', SalesAgent = " & SafeSQL(dtr("SlpCode").ToString.Trim) & ", ShipAgent = " & SafeSQL(dtr("MailStrNo").ToString.Trim) & ", CurrencyCode = " & SafeSQL("").ToString & ", GSTType = " & SafeSQL("") & ", Active = 1, ShipName = " & SafeSQL(dtr("CardName").ToString.Trim) & ", ShipAddr = " & SafeSQL(dtr("Address").ToString.Trim) & ", ShipAddr2 = " & SafeSQL("") & ", ShipAddr3 = '', ShipAddr4 = '', ShipPost = " & SafeSQL(dtr("ZipCode").ToString.Trim) & ", ShipCity = " & SafeSQL(dtr("Country").ToString.Trim) & ", AcCustCode = " & SafeSQL(dtr("CardCode").ToString.Trim) & ", AcBranchRef = '' Where CustNo = " & SafeSQL(dtr("CardCode").ToString.Trim)
                ExecuteSQLAnother(Str)
            Else
                Str = "Insert into Customer(CustNo, CustName, ChineseName, Address, Address2, Address3, Address4, PostCode, City, CountryCode, Phone, ContactPerson, Balance, CreditLimit, ProvisionalBalance, ZoneCode, FaxNo, Email, Website, ICPartner, PriceGroup, Category, " & _
                      "PaymentTerms, PaymentMethod, ShipmentMethod, SalesAgent, ShipAgent, Location, CurrencyCode, GstType, DisplayNo, CustPostGroup, CustType, GSTCustGroup, SearchName, Active, ShipName, ShipAddr, ShipAddr2, ShipAddr3, ShipAddr4, ShipPost, ShipCity, AcCustCode, AcBranchRef, ToPDA) Values " & _
                      "(" & SafeSQL(dtr("CardCode").ToString.Trim) & "," & SafeSQL(dtr("CardName").ToString.Trim) & "," & SafeSQL(dtr("CardName").ToString.Trim) & "," & SafeSQL(dtr("Address").ToString.Trim) & ", " & SafeSQL("") & ", " & SafeSQL("") & ", " & SafeSQL("") & "," & _
                      SafeSQL(dtr("ZipCode").ToString.Trim) & ",''," & SafeSQL(dtr("Country").ToString.Trim) & "," & SafeSQL(dtr("Phone1").ToString.Trim) & "," & SafeSQL(dtr("Phone1").ToString.Trim) & "," & IIf(IsDBNull(dtr("Balance")), 0, dtr("Balance")) & ", " & IIf(IsDBNull(dtr("CreditLine")), 0, dtr("CreditLine")) & ",0,'', " & SafeSQL(dtr("Fax").ToString.Trim) & ", '', '', " & SafeSQL("0") & ", " & SafeSQL(dtr("ListNum").ToString.Trim) & ", " & SafeSQL(dtr("GroupCode").ToString.Trim) & _
                      "," & SafeSQL(dtr("GroupNum").ToString.Trim) & ",'', '', " & SafeSQL(dtr("SlpCode").ToString.Trim) & "," & SafeSQL(dtr("MailStrNo").ToString.Trim) & _
                      ", '', ''," & SafeSQL("") & ",1,'','Customer',''," & SafeSQL(dtr("CardName").ToString.Trim) & ",1, " & SafeSQL(dtr("CardName").ToString.Trim) & ", " & SafeSQL(dtr("Address").ToString.Trim) & ", " & SafeSQL("") & ", '', '', " & SafeSQL(dtr("ZipCode").ToString.Trim) & ", " & SafeSQL(dtr("Country").ToString.Trim) & ", " & SafeSQL(dtr("CardCode").ToString.Trim) & ", '',1)"
                ExecuteSQLAnother(Str)
            End If
            icnt += 1
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
        If bSync = True Then
            UpdateLastTimeStamp("Customer", iValue)
        Else
            InsertLastTimeStamp("Customer", iValue)
        End If
        'dtr = ReadNavRecord("Select * from """ & sNavCompanyName & "Default Dimension"" where ""Table Name""='Customer'")
        'While dtr.Read
        '    If dtr("Dimension Code").ToString = "REGION" Then
        '        ExecuteSQLAnother("Update Customer Set ZoneCode=" & SafeSQL(dtr("Dimension Value Code").ToString) & ", Dimension2=" & SafeSQL(dtr("Dimension Value Code").ToString) & " where CustNo=" & SafeSQL(dtr("No_")))
        '    End If
        'End While
        'dtr.Close()

        ExecuteSQL("Update Customer Set PriceGroup ='ALL' Where (PriceGroup='' or PriceGroup is Null)")
        ExecuteSQL("Update Customer Set AcBillRef ='' Where AcBillRef is Null")
        ExecuteSQL("Update Customer Set ShipName = CustName where (ShipName='' or ShipName is Null)")
        ' ExecuteSQL("Update Customer Set Active=0 where CustName like '%Closed%'")
        ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Customer'")
    End Sub




    Public Sub ImportVendor()
        Dim dtr As SqlDataReader
        Dim icnt As Integer = 1
        Dim iValue As Date = Date.Now
        Dim ivalueLast30 As Date = iValue.AddDays(-600)
        Dim dNewRecord As Int16 = 0
        Dim bSync As Boolean = GetLastTimeStamp("Vendor", iValue, dNewRecord)
        If dNewRecord = 0 Then
            dtr = ReadNavRecord("SELECT * FROM OCRD WHERE (CardType = 'S') and CardCode <> 'WILLY MOTOR CO'")
        Else
            ' dtr = ReadNavRecord("SELECT * FROM OCRD WHERE (CardType = 'S') and CardCode <> 'WILLY MOTOR CO' and (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
            dtr = ReadNavRecord("SELECT * FROM OCRD WHERE (CardType = 'S') and CardCode <> 'WILLY MOTOR CO'")
        End If
        Dim sdVenNo As String = "", Str As String = ""
        While dtr.Read
            sdVenNo = dtr("CardCode").ToString
            If dtr("CardCode").ToString <> "" And IsVendorExists(sdVenNo) = True Then
                Str = "Update Vendor Set VendorName  = " & SafeSQL(dtr("CardName").ToString.Trim) & ", Address = " & SafeSQL(dtr("Address").ToString.Trim) & ", Address2 = " & SafeSQL("") & ", Address3 = '', Address4 = '', PostCode = " & SafeSQL(dtr("ZipCode").ToString.Trim) & ", City = '', CountryCode = " & SafeSQL(dtr("Country").ToString.Trim) & ", PhoneNo = " & SafeSQL(dtr("Phone1").ToString.Trim) & ", Contact =" & SafeSQL(dtr("CntctPrsn").ToString.Trim) & ", FaxNo = " & SafeSQL(dtr("Fax").ToString.Trim) & ", email = " & SafeSQL(dtr("E_Mail").ToString.Trim) & ",CurrencyCode = " & SafeSQL("").ToString & ",Active = 1,  DTG = " & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ",LocationCode = '',PayTerms = '' Where VendorNo = " & SafeSQL(dtr("CardCode").ToString.Trim)
                ' MsgBox(Str)
                ExecuteSQLAnother(Str)
            Else
                Str = "Insert into Vendor(VendorNo, VendorName, ChineseName, Address, Address2, Address3, Address4, PostCode,City,CountryCode,PhoneNo,Contact,FaxNo,Email,Website,Active,DTG,LocationCode,PayTerms) Values " & "(" & SafeSQL(dtr("CardCode").ToString) & "," & SafeSQL(dtr("CardName").ToString) & "," & SafeSQL(dtr("CardName").ToString) & "," & SafeSQL(dtr("Address").ToString) & "," & SafeSQL("") & "," & SafeSQL("") & "," & SafeSQL("") & "," & SafeSQL(dtr("ZipCode").ToString.Trim) & ",''," & SafeSQL(dtr("Country").ToString.Trim) & "," & SafeSQL(dtr("Phone1").ToString.Trim) & "," & SafeSQL(dtr("Phone1").ToString.Trim) & "," & SafeSQL(dtr("Fax").ToString.Trim) & ",'', '',1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & " ,'','')"
                'MsgBox(Str)
                ExecuteSQLAnother(Str)
            End If
            icnt += 1
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
        If bSync = True Then
            UpdateLastTimeStamp("Vendor", iValue)
        Else
            InsertLastTimeStamp("Vendor", iValue)
        End If
        'dtr = ReadNavRecord("Select * from """ & sNavCompanyName & "Default Dimension"" where ""Table Name""='Vendor'")
        'While dtr.Read
        '    If dtr("Dimension Code").ToString = "REGION" Then
        '        ExecuteSQLAnother("Update Customer Set ZoneCode=" & SafeSQL(dtr("Dimension Value Code").ToString) & ", Dimension2=" & SafeSQL(dtr("Dimension Value Code").ToString) & " where CustNo=" & SafeSQL(dtr("No_")))
        '    End If
        'End While
        'dtr.Close()

        'ExecuteSQL("Update Customer Set PriceGroup ='ALL' Where (PriceGroup='' or PriceGroup is Null)")
        'ExecuteSQL("Update Customer Set AcBillRef ='' Where AcBillRef is Null")
        ' ExecuteSQL("Update Customer Set ShipName = CustName where (ShipName='' or ShipName is Null)")
        ' ExecuteSQL("Update Customer Set Active=0 where CustName like '%Closed%'")
        ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Vendor'")
    End Sub

    Public Sub ImportCustGroup()
        Dim dtr As SqlDataReader
        ExecuteSQL("Delete From CustGroup")
        dtr = ReadNavRecord("Select * from OITB")
        While dtr.Read
            If dtr("ItmsGrpCod").ToString <> "" Then
                If IsExists("Select Code from CustGroup where code=" & SafeSQL(dtr("ItmsGrpCod"))) = False Then
                    ExecuteSQLAnother("Insert into CustGroup(Code , Description) Values (" & SafeSQL(dtr("ItmsGrpCod").ToString) & "," & SafeSQL(IIf(dtr("ItmsGrpNam").ToString = "", dtr("ItmsGrpCod").ToString, dtr("ItmsGrpNam").ToString)) & ")")
                Else
                    ExecuteSQLAnother("Update CustGroup set Description=" & SafeSQL(IIf(dtr("ItmsGrpNam").ToString = "", dtr("GroupCode").ToString, dtr("ItmsGrpNam").ToString)) & " Where Code=" & SafeSQL(dtr("ItmsGrpCod").ToString))
                End If
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
    End Sub

    Public Sub ImportCategory()
        Dim dtr As SqlDataReader
        ExecuteSQL("Delete From Category")
        dtr = ReadNavRecord("Select * from OITB")
        While dtr.Read
            If dtr("ItmsGrpCod").ToString <> "" Then
                If IsExists("Select Code from Category where code=" & SafeSQL(dtr("ItmsGrpCod"))) = False Then
                    ExecuteSQLAnother("Insert into Category(Code , Description) Values (" & SafeSQL(dtr("ItmsGrpCod").ToString) & "," & SafeSQL(IIf(dtr("ItmsGrpNam").ToString = "", dtr("ItmsGrpCod").ToString, dtr("ItmsGrpNam").ToString)) & ")")
                Else
                    ExecuteSQLAnother("Update Category set Description=" & SafeSQL(IIf(dtr("ItmsGrpNam").ToString = "", dtr("ItmsGrpCod").ToString, dtr("ItmsGrpNam").ToString)) & " Where Code=" & SafeSQL(dtr("ItmsGrpCod").ToString))
                End If
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
        ExecuteSQL("Update ImportStatus Set Status = 1 where TableName='Category'")
    End Sub

    Public Sub ImportBrand()
        Dim dtr As SqlDataReader
        ExecuteSQL("Delete From Brand")
        dtr = ReadNavRecord("Select * from OMRC")
        While dtr.Read
            If dtr("FirmCode").ToString <> "" Then
                If IsExists("Select Code from Brand where code=" & SafeSQL(dtr("FirmCode"))) = False Then
                    ExecuteSQLAnother("Insert into Brand(Code , Description) Values (" & SafeSQL(dtr("FirmCode").ToString) & "," & SafeSQL(IIf(dtr("FirmName").ToString = "", dtr("FirmCode").ToString, dtr("FirmName").ToString)) & ")")
                Else
                    ExecuteSQLAnother("Update Brand set Description=" & SafeSQL(IIf(dtr("FirmName").ToString = "", dtr("FirmCode").ToString, dtr("FirmName").ToString)) & " Where Code=" & SafeSQL(dtr("FirmCode").ToString))
                End If
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
    End Sub

    Public Sub ImportBarcodes()
        Dim dtr As SqlDataReader
        ExecuteSQL("Delete From Barcodes")

        dtr = ReadNavRecord("Select * from OBCD")
        While dtr.Read
            If dtr("BcdCode").ToString <> "" Then
                If IsExists("Select BarCode from Barcodes where BarCode=" & SafeSQL(dtr("BcdCode").ToString) & " and ItemNo=" & SafeSQL(dtr("ItemCode"))) = False Then
                    ExecuteSQLAnother("Insert into Barcodes(ItemNo, BarCode, UOM, Variant,  DTG) Values (" & SafeSQL(dtr("ItemCode").ToString) & "," & SafeSQL(dtr("BcdCode").ToString) & ",''," & SafeSQL("") & ", GetDate())")
                End If
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing

        ExecuteSQLAnother("Update Barcodes set UOM=Item.BaseUOM from Barcodes, Item where Barcodes.ItemNo=Item.ItemNo")

    End Sub

    Public Sub ImportProduct()
        Dim dtr As SqlDataReader

        dtr = ReadNavRecord("Select * from OITM where InvntItem='Y'")
        Dim icnt As Integer = 1
        While dtr.Read
            If dtr("ItemCode").ToString <> "" Then
                Dim sCatcode, sBrand As String
                If dtr("FirmCode").ToString <> "" Then
                    sBrand = dtr("FirmCode").ToString
                Else
                    sBrand = "STD"
                End If
                If dtr("ItmsGrpCod").ToString <> "" Then
                    sCatcode = dtr("ItmsGrpCod").ToString
                Else
                    sCatcode = "STD"
                End If
                Dim _active As Integer = 0
                If dtr("FrozenFor").ToString = "Y" Then
                    _active = 0
                ElseIf dtr("FrozenFor").ToString = "N" Then
                    _active = 1
                End If

                If IsItemExists(dtr("ItemCode")) Then
                    ExecuteSQL("Update Item Set Description = " & SafeSQL(dtr("ItemName").ToString) & ", ShortDesc = " & SafeSQL(IIf(dtr("ItemName").ToString = "", dtr("ItemName").ToString, dtr("ItemName").ToString)) & ", BaseUOM = " & SafeSQL(dtr("InvntryUom").ToString.ToUpper) & ", ItemName=" & SafeSQL(dtr("ItemName").ToString) & ", UnitPrice = " & IIf(IsDBNull(dtr("LstEvlPric")), 0, dtr("LstEvlPric")) & ", VendorNo = " & SafeSQL("") & ", VendorItemNo = " & SafeSQL("") & ", Active = " & _active & " , Category =  " & SafeSQL(sCatcode) & ", SubCategory = " & SafeSQL(sBrand) & ", Brand=" & SafeSQL(sBrand) & ", Barcode = " & SafeSQL(dtr("CodeBars").ToString) & ", Dimension2 = " & SafeSQL("") & ", CostPrice = " & IIf(IsDBNull(dtr("LastPurPrc")), 0, dtr("LastPurPrc")) & ", CompanyName='STD' where ItemNo = " & SafeSQL(dtr("ItemCode").ToString))
                    If Trim(dtr("PurPackMsr").ToString) <> Trim(dtr("InvntryUom").ToString) Then
                        ExecuteSQL("Delete from UOM where ItemNo=" & SafeSQL(dtr("ItemCode").ToString) & " and UOM=" & SafeSQL(dtr("InvntryUom").ToString.ToUpper))
                        ExecuteSQLAnother("Insert into UOM(ItemNo, Uom , BaseQty, DTG) Values (" & SafeSQL(dtr("ItemCode").ToString) & "," & SafeSQL(dtr("InvntryUom").ToString.ToUpper) & ",1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
                        ExecuteSQL("Delete from UOM where ItemNo=" & SafeSQL(dtr("ItemCode").ToString) & " and UOM=" & SafeSQL(dtr("PurPackMsr").ToString))
                        ExecuteSQLAnother("Insert into UOM(ItemNo, Uom , BaseQty, DTG) Values (" & SafeSQL(dtr("ItemCode").ToString) & "," & SafeSQL(dtr("PurPackMsr").ToString.ToUpper) & "," & IIf(IsDBNull(dtr("SalFactor2")), 1, dtr("SalFactor2")) & "," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
                    Else
                        ExecuteSQL("Delete from UOM where ItemNo=" & SafeSQL(dtr("ItemCode").ToString) & " and UOM=" & SafeSQL(dtr("InvntryUom").ToString))
                        ExecuteSQLAnother("Insert into UOM(ItemNo, Uom , BaseQty, DTG) Values (" & SafeSQL(dtr("ItemCode").ToString) & "," & SafeSQL(dtr("InvntryUom").ToString.ToUpper) & ",1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
                    End If

                Else
                    ExecuteSQL("Insert into Item(ItemNo, Description, ItemName, ShortDesc, ChineseDesc, BaseUOM, UnitPrice, VendorNo, VendorItemNo, AllowInvDiscount, Active, Category, DisplayNo, Barcode, ItemPostGroup, SubCategory, Brand, CompanyNo, ToPDA,LotTracking ,ChangePr, Dimension1, Dimension2, CostPrice, CompanyName) Values (" & SafeSQL(dtr("ItemCode").ToString) & "," & SafeSQL(dtr("ItemName").ToString) & "," & SafeSQL(dtr("ItemName").ToString) & "," & SafeSQL(IIf(dtr("ItemName").ToString = "", dtr("ItemName").ToString, dtr("ItemName").ToString)) & ",''," & SafeSQL(dtr("InvntryUom").ToString.ToUpper) & ", " & IIf(IsDBNull(dtr("LstEvlPric")), 0, dtr("LstEvlPric")) & ", " & SafeSQL("") & ", " & SafeSQL("") & ", 1 ," & _active & " , " & SafeSQL(sCatcode) & ",0," & SafeSQL(dtr("CodeBars").ToString) & ",''," & SafeSQL(sBrand) & "," & SafeSQL(sBrand) & ",'',1,0,0," & SafeSQL("") & "," & SafeSQL("") & "," & IIf(IsDBNull(dtr("LastPurPrc")), 0, dtr("LastPurPrc")) & ", 'STD')")
                    If Trim(dtr("PurPackMsr").ToString) <> Trim(dtr("InvntryUom").ToString) Then
                        ExecuteSQL("Delete from UOM where ItemNo=" & SafeSQL(dtr("ItemCode").ToString) & " and UOM=" & SafeSQL(dtr("InvntryUom").ToString))
                        ExecuteSQLAnother("Insert into UOM(ItemNo, Uom , BaseQty, DTG) Values (" & SafeSQL(dtr("ItemCode").ToString) & "," & SafeSQL(dtr("InvntryUom").ToString.ToUpper) & ",1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
                        ExecuteSQL("Delete from UOM where ItemNo=" & SafeSQL(dtr("ItemCode").ToString) & " and UOM=" & SafeSQL(dtr("PurPackMsr").ToString))
                        ExecuteSQLAnother("Insert into UOM(ItemNo, Uom , BaseQty, DTG) Values (" & SafeSQL(dtr("ItemCode").ToString) & "," & SafeSQL(dtr("PurPackMsr").ToString.ToUpper) & "," & IIf(IsDBNull(dtr("SalFactor2")), 1, dtr("SalFactor2")) & "," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
                    Else
                        ExecuteSQL("Delete from UOM where ItemNo=" & SafeSQL(dtr("ItemCode").ToString) & " and UOM=" & SafeSQL(dtr("InvntryUom").ToString))
                        ExecuteSQLAnother("Insert into UOM(ItemNo, Uom , BaseQty, DTG) Values (" & SafeSQL(dtr("ItemCode").ToString) & "," & SafeSQL(dtr("InvntryUom").ToString.ToUpper) & ",1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
                    End If
                End If
            End If
            icnt += 1
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
        ExecuteSQL("Update Item set Active=0 where Description like '%DO NOT%'")
        ExecuteSQL("Update Item set CostPrice=0 where CostPrice is Null")
        ExecuteSQL("Update Item set ShortDesc = Description where ShortDesc = ''")
        ExecuteSQL("Update ImportStatus Set Status = 0 where TableName='Product'")
    End Sub

    Public Sub ImportItemPrice()
        Dim dtr As SqlDataReader
        Dim sType As String = ""
        Dim sPriceGroup As String = ""
        Dim sItemNo As String = ""
        Dim sUOm As String = ""
        Dim dQty As Double = 0
        ExecuteSQL("Delete from ItemPr")
        dtr = ReadNavRecord("Select * from ITM1 where Price > 0")
        While dtr.Read
            sType = "Customer Price Group"
            ExecuteSQLAnother("Insert into ItemPr(PriceGroup, ItemNo, UnitPrice, SalesType, Minqty, VariantCode, UOM, FromDate, ToDate) Values (" & SafeSQL(dtr("PriceList").ToString.Trim) & "," & SafeSQL(dtr("ItemCode").ToString.Trim) & "," & IIf(IsDBNull(dtr("Price")), 0, dtr("Price")) & "," & SafeSQL(sType) & ",0," & SafeSQL("") & "," & SafeSQL("") & "," & SafeSQL(Format(Date.Now, "yyyyMMdd 00:00:00")) & "," & SafeSQL(Format(Date.Now, "yyyyMMdd 23:59:59")) & ")")
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
        ExecuteSQL("Delete from ItemPr where UnitPrice = 0")
        ExecuteSQL("Update ItemPr Set MinPrice = 0 where MinPrice is Null")
        ExecuteSQL("Update ItemPr set UOM = Item.BaseUOM from Item where Item.ItemNo = ItemPr.ItemNo and ItemPr.UOM = ''")
    End Sub

    Public Sub ImportLocation()
        Dim dtr As SqlDataReader
        dtr = ReadNavRecord("Select * from OWHS")
        While dtr.Read
            If IsExists("Select Code from Location where code=" & SafeSQL(dtr("WhsCode"))) = False Then
                ExecuteSQLAnother("Insert into Location(Code, Name, DTG) Values (" & SafeSQL(dtr("WhsCode").ToString) & "," & SafeSQL(dtr("WhsName").ToString) & ", getdate())")
            Else
                ExecuteSQLAnother("Update Location set Name=" & SafeSQL(IIf(dtr("WhsName").ToString = "", dtr("WhsCode").ToString, dtr("WhsName").ToString)) & " Where Code=" & SafeSQL(dtr("WhsCode").ToString))
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
    End Sub

    Public Sub ImportSalesAgent()
        Dim dtr As SqlDataReader
        'ExecuteSQL("Update SalesAgent Set Active = 0")
        dtr = ReadNavRecord("Select * from OSLP where Locked='N'")
        While dtr.Read
            If dtr("SlpCode").ToString <> "" And dtr("Locked").ToString = "N" Then
                If IsAgentExists(dtr("SlpCode").ToString) = False Then
                    ExecuteSQL("Insert into SalesAgent(Code, Name, Phone,Email, Password, UserID, Access, Active, SalesTarget) Values (" & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpName").ToString) & "," & SafeSQL("") & "," & SafeSQL("") & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & ",1,1,0)")
                Else
                    ExecuteSQL("Update SalesAgent Set Name = " & SafeSQL(dtr("SlpName").ToString) & ", Active = 1 Where Code = " & SafeSQL(dtr("SlpCode").ToString))
                End If
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
        ExecuteSQL("Update SalesAgent Set Active = 1 where Code = 'ADMIN'")
    End Sub



    Public Sub ImportUOM()
        Dim dtr As SqlDataReader
        dtr = ReadNavRecord("Select  ""Item No_"", Code, ""Qty_ per Unit of Measure"" from """ & sNavCompanyName & "Item Unit of Measure"" A, """ & sNavCompanyName & "Item""  B Where A.""Item No_"" = B.No_ ")
        While dtr.Read
            If IsExists("Select ItemNo from UOM where ItemNo=" & SafeSQL(dtr("Item No_").ToString.Trim) & " and Uom=" & SafeSQL(dtr("Code").ToString.Trim)) = False Then
                ExecuteSQLAnother("Insert into UOM(ItemNo, Uom , BaseQty, DTG) Values (" & SafeSQL(dtr("Item No_").ToString) & "," & SafeSQL(dtr("Code").ToString) & "," & SafeSQL(dtr("Qty_ per Unit of Measure").ToString) & "," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
            Else
                ExecuteSQLAnother("Update UOM set BaseQty=" & SafeSQL(dtr("Qty_ per Unit of Measure").ToString) & " Where ItemNo=" & SafeSQL(dtr("Item No_").ToString.Trim) & " and UOM=" & SafeSQL(dtr("Code").ToString.Trim))
            End If
        End While
        dtr.Close()
    End Sub

    Public Sub ImportPayMethod()

    End Sub


    Public Sub ImportPayterms()
        Dim dtr As SqlDataReader
        dtr = ReadNavRecord("Select * from OCTG")
        While dtr.Read
            If IsExists("Select Code from PayTerms where code=" & SafeSQL(dtr("GroupNum"))) = False Then
                ExecuteSQLAnother("Insert into PayTerms(Code , Description, DueDateCalc, DisDateCalc, DiscountPercent,Active,DTG) Values (" & SafeSQL(dtr("GroupNum").ToString) & "," & SafeSQL(dtr("PymntGroup").ToString) & "," & SafeSQL(dtr("ExtraDays").ToString + "D") & "," & SafeSQL(dtr("ExtraDays").ToString + "D") & ",0,1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
            Else
                ExecuteSQLAnother("Update PayTerms set Description=" & SafeSQL(IIf(dtr("PymntGroup").ToString = "", dtr("GroupNum").ToString, dtr("PymntGroup").ToString)) & ", DueDateCalc =" & SafeSQL(dtr("ExtraDays").ToString + "D") & ", DisDateCalc=" & SafeSQL(dtr("ExtraDays").ToString + "D") & " Where Code=" & SafeSQL(dtr("GroupNum").ToString))
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
    End Sub


    Public Sub ImportPriceGroup()
        Dim dtr As SqlDataReader
        dtr = ReadNavRecord("Select * from OPLN")
        While dtr.Read
            If IsExists("Select Code from PriceGroup where code=" & SafeSQL(dtr("ListNum"))) = False Then
                ExecuteSQLAnother("Insert into PriceGroup(Code , Description, InvoiceDiscount, LineDiscount, DTG) Values (" & SafeSQL(dtr("ListNum").ToString) & "," & SafeSQL(dtr("ListName").ToString) & ",1,1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
            Else
                ExecuteSQLAnother("Update PriceGroup set Description=" & SafeSQL(IIf(dtr("ListName").ToString = "", dtr("ListNum").ToString, dtr("ListName").ToString)) & " ,DTG=GetDate() Where Code=" & SafeSQL(dtr("ListNum").ToString))
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
    End Sub

    Public Sub ImportInvoice()
        Dim dtr As SqlDataReader
        Dim rs As SqlDataReader
        Dim dNewRecord As Int16 = 0
        Dim iValue As Date = Date.Now
        Dim ivalueLast30 As Date = iValue.AddDays(-30)
        Dim bSync As Boolean = GetLastTimeStamp("Invoice", iValue, dNewRecord)
        Dim arrList As New ArrayList
        If dNewRecord = 0 Then
            'dtr = ReadNavRecord("SELECT * FROM OInv where (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")))
            dtr = ReadNavRecord("Select * from OInv where (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        Else
            dtr = ReadNavRecord("Select * from OInv where (UpdateDate >= " & SafeSQL(Format(iValue, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        End If
        While dtr.Read
            Dim dDisAmt As Double = 0
            rs = ReadRecord("Select PaidAmt from Invoice where InvNo= " & SafeSQL(dtr("DocNum")))
            If rs.Read = True Then
                rs.Close()
                ExecuteSQL("Update Invoice set PaidAmt = " & dtr("PaidToDate") & ", custid = " & SafeSQL(dtr("cardcode")) & ", DTG = " & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & " where InvNo =" & SafeSQL(dtr("DocNum")))
            Else
                Dim aInv As New ArrInvoice
                aInv.InvNo = dtr("DocNum").ToString
                aInv.InvDate = dtr("DocDate")
                aInv.OrdNo = dtr("Ref1").ToString
                aInv.CustID = dtr("CardCode").ToString
                aInv.AgentID = dtr("SlpCode").ToString
                aInv.Discount = 0
                aInv.CurCode = dtr("DocCur").ToString
                aInv.CurExRate = dtr("DocRate")
                '  inv()
                aInv.GSTAmt = dtr("VatSum")
                aInv.Subtotal = dtr("DocTotal") - dtr("VatSum")
                aInv.TotalAmt = dtr("DocTotal")
                aInv.PaidAmt = dtr("PaidToDate")
                aInv.payterms = dtr("GroupNum").ToString
                arrList.Add(aInv)
                rs.Close()
                ExecuteSQL("Insert into Invoice(InvNo, InvDt, DONo, DoDt ,OrdNo, CustId, AgentID, Discount, SubTotal, GSTAmt, TotalAmt, PaidAmt, Void, PrintNo, PayTerms, CurCode, CurExRate, PONo, Exported,DTG,CompanyName,AcBillRef) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("Ref1").ToString) & "," & SafeSQL(dtr("CardCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & dDisAmt & ", " & dtr("DocTotal") - dtr("VatSum") & ", " & dtr("VatSum") & " , " & dtr("DocTotal") & ", " & dtr("PaidToDate") & ",0,1," & SafeSQL(dtr("GroupNum").ToString) & "," & SafeSQL(dtr("DocCur").ToString) & "," & IIf(IsDBNull(dtr("DocRate")), 1, dtr("DocRate")) & ",'',1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & "," & SafeSQL("STD") & "," & SafeSQL(dtr("CardCode").ToString) & ")")
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing

        If rs Is Nothing = False Then
            rs.Dispose()
            rs = Nothing
        End If

        ImportInvItem(arrList)
        ExecuteSQLAnother("Update InvItem set UOM=Item.BaseUOM from InvItem, Item where InvItem.ItemNo= Item.ItemNo and (InvItem.UOM is Null or InvItem.UOM='')")
        'For iIndex As Integer = 0 To arrList.Count - 1
        '    Dim aP As New ArrInvoice
        '    aP = arrList(iIndex)
        '    rs = ReadRecord("Select SUM(SubAmt) as Amount, sum(gstamt) as Gst from InvItem where InvNo= " & SafeSQL(aP.InvNo))
        '    If rs.Read = True Then
        '        ExecuteSQLAnother("Update Invoice set SubTotal = " & IIf(IsDBNull(rs("Amount")), 0, rs("Amount")) & ", " & _
        '                          "Gstamt = " & IIf(IsDBNull(rs("Gst")), 0, rs("Gst")) & ", TotalAmt = " & IIf(IsDBNull(rs("Amount")), 0, rs("Amount")) + IIf(IsDBNull(rs("gst")), 0, rs("gst")) & " where InvNo =" & SafeSQL(aP.InvNo))
        '    End If
        '    rs.Close()
        'Next
        If bSync = True Then
            UpdateLastTimeStamp("Invoice", iValue)
        Else
            InsertLastTimeStamp("Invoice", iValue)
        End If
    End Sub

    Public Sub ImportPurchaseOrder()
        Dim dtr As SqlDataReader
        Dim rs As SqlDataReader
        Dim dNewRecord As Int16 = 0
        Dim iValue As Date = Date.Now

        Dim ivalueLast30 As Date = iValue.AddDays(-10)
        '  Dim ivalueLast30 As Date = iValue.AddDays(-30)

        Dim bSync As Boolean = GetLastTimeStamp("PurchaseOrder", iValue, dNewRecord)
        Dim arrList As New ArrayList
        'If dNewRecord = 0 Then


        '    'dtr = ReadNavRecord("SELECT * FROM OPOR where (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")))
        Dim sSQL As String = ""
        If bDeleteIns = True Then

            ExecuteSQL("Update PODET Set Qty=0")
            sSQL = "Select * from OPOR where DocStatus='O' order by UpdateDate"
            dtr = ReadNavRecord(sSQL)
        Else
            sSQL = "Select * from OPOR where (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate"
            dtr = ReadNavRecord(sSQL)
        End If

        Dim aPO As New ArrPurchaseOrder

        'System.IO.File.AppendAllText("C:\ImportLog.txt", "PO : " & Date.Now.ToString & vbCrLf & sSQL & vbCrLf)
        'Else

        '    dtr = ReadNavRecord("Select * from OPOR where (UpdateDate >= " & SafeSQL(Format(iValue, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        'End If
        While dtr.Read
            'MsgBox("Entered purchaseorder1")
            Dim dDisAmt As Double = 0
            rs = ReadRecord("Select PONo from POHDR where PONo= " & SafeSQL(dtr("DocNum")))
            If rs.Read = True Then
                aPO.PONo = dtr("DocNum").ToString
                aPO.DocEntry = dtr("DocEntry").ToString
                'aPO.PODt = Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")
                aPO.PODt = dtr("DocDate")
                aPO.VendorId = dtr("SlpCode").ToString
                aPO.AgentID = dtr("SlpCode").ToString
                aPO.Discount = 0
                aPO.CurCode = dtr("DocCur").ToString
                aPO.CurExRate = dtr("DocRate")
                ' aPO.DTG = Format(Date.Now, "yyyyMMdd HH:mm:ss")
                aPO.DTG = Date.Now
                aPO.PrintNo = ""
                aPO.GSTAmt = dtr("VatSum")
                aPO.SubTotal = dtr("DocTotal") - dtr("VatSum")
                aPO.TotalAmt = dtr("DocTotal")
                aPO.Void = dtr("Canceled")
                aPO.PayTerms = dtr("GroupNum").ToString
                aPO.Exported = ""
                'aPO.DeliveryDate = Format(dtr("DocDueDate"), "yyyyMMdd HH:mm:ss")
                aPO.DeliveryDate = dtr("DocDueDate")
                aPO.LocationCode = ""
                aPO.ExternalDocNo = ""
                aPO.Remarks = ""
                aPO.POType = ""
                aPO.ContainerNo = ""
                aPO.Department = ""
                aPO.ManufacturerCode = ""
                aPO.CompanyName = ""
                arrList.Add(aPO)

                rs.Close()
                'MsgBox("Entered purchaseorderif")

                ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Update - PO'," & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL("") & ")")

                If dtr("DocStatus") = "C" Then
                    ExecuteSQL("Update POHDR set Void=1 where PONo =" & SafeSQL(dtr("DocNum")))
                Else
                    ExecuteSQL("Update POHDR set Void = 0, PONo = " & dtr("DocNum") & ", Vendorid = " & SafeSQL(dtr("cardcode")) & ", DTG = " & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & " where PONo =" & SafeSQL(dtr("DocNum")))
                End If

            Else
                '    MsgBox("Entered purchaseorderelse")

                aPO.PONo = dtr("DocNum").ToString
                aPO.DocEntry = dtr("DocEntry").ToString
                'aPO.PODt = Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")
                aPO.PODt = dtr("DocDate")
                aPO.VendorId = dtr("SlpCode").ToString
                aPO.AgentID = dtr("SlpCode").ToString
                aPO.Discount = 0
                aPO.CurCode = dtr("DocCur").ToString
                aPO.CurExRate = dtr("DocRate")
                ' aPO.DTG = Format(Date.Now, "yyyyMMdd HH:mm:ss")
                aPO.DTG = Date.Now
                aPO.PrintNo = ""
                aPO.GSTAmt = dtr("VatSum")
                aPO.SubTotal = dtr("DocTotal") - dtr("VatSum")
                aPO.TotalAmt = dtr("DocTotal")
                aPO.Void = dtr("Canceled")
                aPO.PayTerms = dtr("GroupNum").ToString
                aPO.Exported = ""
                'aPO.DeliveryDate = Format(dtr("DocDueDate"), "yyyyMMdd HH:mm:ss")
                aPO.DeliveryDate = dtr("DocDueDate")
                aPO.LocationCode = ""
                aPO.ExternalDocNo = ""
                aPO.Remarks = ""
                aPO.POType = ""
                aPO.ContainerNo = ""
                aPO.Department = ""
                aPO.ManufacturerCode = ""
                aPO.CompanyName = ""
                arrList.Add(aPO)

                rs.Close()
                '   MsgBox("entering insert")
                ExecuteSQL("Insert into POHdr(PONo, PODt, VendorId, AgentID, Discount, SubTotal, GSTAmt, TotalAmt, Void, PrintNo, PayTerms, CurCode, CurExRate, Exported,DTG, DeliveryDate,LocationCode, ExternalDocNo, Remarks, POType, ContainerNo, Department, ManufacturerCode, CompanyName) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("cardcode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & dDisAmt & ", 0, 0, 0, 0,1," & SafeSQL(dtr("GroupNum").ToString) & "," & SafeSQL(dtr("DocCur").ToString) & "," & dtr("DocRate").ToString & ",1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(Format(dtr("DocDueDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL("") & "," & SafeSQL("") & "," & SafeSQL("") & ",'Purchase'," & SafeSQL("") & "," & SafeSQL("") & "," & SafeSQL("") & "," & SafeSQL("") & ")")

                ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'Insert - PO'," & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL("") & ")")

            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
        ImportPODET(arrList)
        If rs Is Nothing = False Then
            rs.Dispose()
            rs = Nothing
        End If


        '        ImportPODET(arrList)
        If bSync = True Then
            UpdateLastTimeStamp("PurchaseOrder", iValue)
        Else
            InsertLastTimeStamp("PurchaseOrder", iValue)
        End If
    End Sub

    Public Sub ImportPODET(ByVal arrList As ArrayList)
        'MsgBox(arrList.Count)

        'Dim sInvNos As String = "''"
        'For idx As Integer = 0 To arrList.Count - 1
        '    Dim aPr As ArrInvoice
        '    aPr = arrList(idx)
        '    sInvNos &= "," & SafeSQL(aPr.InvNo)
        'Next

        Dim dtr As SqlDataReader

        ' ExecuteSQL("Delete from PODET where PONo in (" & sInvNos & ")")

        'System.IO.File.AppendAllText("C:\ImportLog.txt", "PO Det: " & Date.Now.ToString & vbCrLf & arrList.Count & vbCrLf)

        For iIndex As Integer = 0 To arrList.Count - 1

            Dim aPo As ArrPurchaseOrder
            aPo = arrList(iIndex)
            '   MsgBox(aPo.PONo)


            ExecuteSQL("Update PODET Set Qty=0 where PONo= " & SafeSQL(aPo.PONo))

            dtr = ReadNavRecord("Select * from POR1 where DocEntry = " & SafeSQL(aPo.DocEntry) & " order by LineNum")
            While dtr.Read
                'MsgBox(dtr("DocEntry"))
                '  MsgBox(aPo.PONo)


                If dtr("DocEntry").ToString = aPo.DocEntry Then

                    Dim sEXESQL As String = ""
                    If IsPODetExists(aPo.PONo.ToString, dtr("Linenum")) = True Then

                        sEXESQL = "Update PODet Set ItemNo=" & SafeSQL(dtr("ItemCode").ToString) & ", Location=" & SafeSQL(dtr("WhsCode").ToString) & ", Description=" & SafeSQL(dtr("Dscription").ToString) & ", UOM='', Qty =" & SafeSQL(dtr("Quantity").ToString) & " , InvnQty=" & SafeSQL(dtr("InvQty").ToString) & " where  PONo= " & SafeSQL(aPo.PONo.ToString) & " and [LineNo]=" & dtr("Linenum")
                    Else
                        sEXESQL = "Insert into PODet(PONo, [LineNo], [AttachedToLineNo], ItemNo, UOM, Qty, InvnQty, Foc, Price, DisPer, DisPr, Discount, SubAmt, GstAmt, DeliQty, BaseQty, VariantCode, Description, OutLet, BinCode, Location, ParentNo, CompanyName) Values (" & SafeSQL(aPo.PONo.ToString) & ", " & SafeSQL(dtr("Linenum").ToString) & ", " & SafeSQL(dtr("Flags").ToString) & "," & SafeSQL(dtr("ItemCode").ToString) & ", " & SafeSQL("") & ", " & SafeSQL(dtr("Quantity").ToString) & ", " & SafeSQL(dtr("InvQty").ToString) & ", 0, " & SafeSQL(dtr("DiscPrcnt").ToString) & ", " & SafeSQL(dtr("DiscPrcnt").ToString) & ", 0, " & SafeSQL(dtr("DiscPrcnt")) & ", " & SafeSQL(dtr("LineTotal").ToString) & ", " & SafeSQL(dtr("TotalFrgn")) & ", " & SafeSQL(dtr("DelivrdQty").ToString) & ", " & "1" & ",''," & SafeSQL(dtr("Dscription").ToString) & "," & SafeSQL("") & "," & SafeSQL("") & "," & SafeSQL(dtr("WhsCode").ToString) & ",''," & SafeSQL("") & ")"
                    End If


                    'System.IO.File.AppendAllText("C:\ImportLog.txt", "PO Det : " & Date.Now.ToString & vbCrLf & sEXESQL & vbCrLf)

                    ExecuteSQLAnother(sEXESQL)

                    Try
                        ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'PO Det Query :'," & SafeSQL(aPo.PONo.ToString) & "," & SafeSQL(sEXESQL) & ")")
                    Catch ex As Exception

                    End Try



                    ' ExecuteSQL("Insert into InvItem(InvNo, [LineNo], ItemNo, UOM, Qty, Foc, Price, DisPer, DisPr, Discount, SubAmt, GstAmt, DeliQty, BaseUOM, BaseQty, Description, ColorRemarks) Values (" & SafeSQL(dtr("Ref1").ToString) & ", " & dtr("DocLineNum").ToString & ", " & SafeSQL(dtr("ItemCode").ToString) & ", " & SafeSQL("") & ", " & dtr("OutQty").ToString & ", 0, " & dtr("Price").ToString & ", 0, 0, 0, " & dtr("OutQty").ToString * dtr("Price").ToString & ", 0, " & dtr("OutQty").ToString & ", " & SafeSQL("") & ", 1," & SafeSQL(dtr("Dscription").ToString) & ",'')")
                    'Exit For
                End If
            End While
            dtr.Close()
        Next


        If dtr Is Nothing = False Then
            dtr.Dispose()
            dtr = Nothing
        End If

        ExecuteSQLAnother("Update PODet set UOM=UOM.UOM from PODEt, UOM where PODEt.ItemNo= UOM.ItemNo and UOM.BaseQty<>1")

        ExecuteSQLAnother("Update PODet set UOM=Item.BaseUOM from PODEt, Item where PODEt.ItemNo= Item.ItemNo and PODet.Qty=PoDet.InvnQty")




    End Sub


    Public Sub ImportTransferOrder()
        Dim dtr As SqlDataReader
        Dim rs As SqlDataReader
        Dim dNewRecord As Int16 = 0
        Dim iValue As Date = Date.Now
        Dim ivalueLast30 As Date = iValue.AddDays(-30)
        Dim bSync As Boolean = GetLastTimeStamp("TransferOrder", iValue, dNewRecord)
        Dim arrList As New ArrayList
        If dNewRecord = 0 Then
            'dtr = ReadNavRecord("SELECT * FROM OWTR where (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")))
            dtr = ReadNavRecord("Select * from OWTR where (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        Else
            dtr = ReadNavRecord("Select * from OWTR where (UpdateDate >= " & SafeSQL(Format(iValue, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        End If
        While dtr.Read
            Dim dDisAmt As Double = 0
            rs = ReadRecord("Select ordno from transferhdr where Transno= " & SafeSQL(dtr("DocNum")))
            If rs.Read = True Then
                rs.Close()
                ExecuteSQL("Update transferhdr set Transno = " & dtr("DocNum") & ", custid = " & SafeSQL(dtr("cardcode")) & ", DTG = " & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & " where ordno =" & SafeSQL(dtr("DocNum")))
            Else
                Dim atrans As New ArrTrans
                atrans.transno = dtr("DocNum").ToString
                atrans.DocEntry = dtr("DocEntry").ToString
                arrList.Add(atrans)
                rs.Close()
                ' ExecuteSQL("Insert into Invoice(InvNo, InvDt, DONo, DoDt ,OrdNo, CustId, AgentID, Discount, SubTotal, GSTAmt, TotalAmt, PaidAmt, Void, PrintNo, PayTerms, CurCode, CurExRate, PONo, Exported,DTG,CompanyName,AcBillRef) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("Ref1").ToString) & "," & SafeSQL(dtr("CardCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & dDisAmt & ", " & dtr("DocTotal") - dtr("VatSum") & ", " & dtr("VatSum") & " , " & dtr("DocTotal") & ", " & dtr("PaidToDate") & ",0,1," & SafeSQL(dtr("GroupNum").ToString) & "," & SafeSQL(dtr("DocCur").ToString) & "," & IIf(IsDBNull(dtr("DocRate")), 1, dtr("DocRate")) & ",'',1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & "," & SafeSQL("STD") & "," & SafeSQL(dtr("CardCode").ToString) & ")")
                'ExecuteSQL("Insert into OrderHdr (OrdNo, OrdDt, CustId, PONo, AgentId, Discount, SubTotal, GSTAmt, TotalAmt, PayTerms, CurCode, CurExRate,Delivered,void,exported,DeliveryDate, DTG, Remarks) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("cardcode").ToString) & "," & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(dtr("Ref1").ToString) & "," & SafeSQL(0) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & ", 0, " & "1,0,0,0" & "," & SafeSQL(Format(Date.Now, "yyyyMMdd 00:00:00")).ToString & "," & SafeSQL(Format(Date.Now, "yyyyMMdd 00:00:00")).ToString & "," & SafeSQL(dtr("CardCode").ToString) & ")")
                ExecuteSQL("Insert into TransferHdr(TransNo, TransDt, FromLoc, ToLoc, TransStatus, InTransitCode, DTG, Exported, RGNo, ReceiptDate, ShipmentDate, IsNavisionTO) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL("Transfer-from Code").ToString & "," & SafeSQL("Transfer-to Code").ToString & ",1, " & SafeSQL("In-Transit Code").ToString & "," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ", 1," & SafeSQL("") & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(Format(dtr("DocDueDate"), "yyyyMMdd HH:mm:ss")) & ",1)")
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing

        If rs Is Nothing = False Then
            rs.Dispose()
            rs = Nothing
        End If


        Importtransferdet(arrList)
        ' ExecuteSQLAnother("Update InvItem set UOM=Item.BaseUOM from InvItem, Item where InvItem.ItemNo= Item.ItemNo and (InvItem.UOM is Null or InvItem.UOM='')")

        If bSync = True Then
            UpdateLastTimeStamp("TransferOrder", iValue)
        Else
            InsertLastTimeStamp("TransferOrder", iValue)
        End If
    End Sub


    Public Sub ImportDeliveryorder()
        Dim dtr As SqlDataReader
        Dim rs As SqlDataReader
        Dim dNewRecord As Int16 = 0
        Dim iValue As Date = Date.Now
        Dim ivalueLast30 As Date = iValue.AddDays(-2)
        Dim bSync As Boolean = GetLastTimeStamp("DeliveryOrder", iValue, dNewRecord)
        Dim arrList As New ArrayList
        Dim aord As New Arrord
        Dim sSQL As String = ""

        Dim sQrySQL As String = ""


      



        If bDeleteIns = True Then
            sSQL = "Select * from ODLN where DocStatus='O' (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & " or UpdateDate is null)  order by UpdateDate"

            'ExecuteSQL("Update OrdItem Set Qty=0")
        Else
            sSQL = "Select * from ODLN where  (UpdateDate >= " & SafeSQL(Format(iValue, "yyyyMMdd")) & " or CreateDate >= " & SafeSQL(Format(iValue, "yyyyMMdd")) & ") order by DocNum Desc"

        End If


        'System.IO.File.AppendAllText("C:\ImportLog.txt", "SO : " & Date.Now.ToString & vbCrLf & sSQL & vbCrLf)

        dtr = ReadNavRecord(sSQL)
        While dtr.Read

            '  sDONo = dtr("DocNum").ToString
            Dim dDisAmt As Double = 0
            rs = ReadRecord("Select ordno from orderhdr where ordno= " & SafeSQL(dtr("DocNum")))

            If rs.Read = True Then

                aord.Ordno = dtr("DocNum")
                aord.DocEntry = dtr("DocEntry")
                arrList.Add(aord)
                rs.Close()

                If dtr("DocStatus") = "C" Then
                    ExecuteSQL("Update orderhdr set Void=1 where ordno =" & SafeSQL(dtr("DocNum")))
                    Try
                        ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'DO Cancel :'," & SafeSQL(dtr("DocNum")) & "," & SafeSQL("") & ")")
                    Catch ex As Exception

                    End Try

                Else


                    Try
                        
                        ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'DO Update :'," & SafeSQL(dtr("DocNum")) & "," & SafeSQL("") & ")")
                    Catch ex As Exception

                    End Try

                

                    sQrySQL = "Update orderhdr set OrdDt=" & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & ", tempVehicleID=" & SafeSQL(dtr("U_A_TRK").ToString) & ", Remarks=" & SafeSQL(dtr("CardCode").ToString) & ", DeliveryDate=" & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & ", Custid = " & SafeSQL(dtr("cardcode")) & ", DTG = " & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & " where ordno =" & SafeSQL(dtr("DocNum"))
                    ExecuteSQL(sQrySQL)
                End If

            Else

                aord.Ordno = dtr("DocNum")
                aord.DocEntry = dtr("DocEntry").ToString
                arrList.Add(aord)
                rs.Close()

                

                sQrySQL = "Insert into OrderHdr (OrdNo, OrdDt, CustId, PONo, AgentId, Discount, SubTotal, GSTAmt, TotalAmt, PayTerms, CurCode, CurExRate,Delivered,void,exported,DeliveryDate, DTG, Remarks, IsConfirmed, tempVehicleID, Attn) Values (" & SafeSQL(dtr("DocNum")) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("cardcode").ToString) & "," & SafeSQL(dtr("DocNum")) & "," & SafeSQL(dtr("Ref1").ToString) & "," & SafeSQL(0) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & ", 0, " & "1,0,0,0" & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(Format(Date.Now, "yyyyMMdd 00:00:00")).ToString & "," & SafeSQL(dtr("CardCode").ToString) & " ,1, " & SafeSQL(dtr("U_A_TRK").ToString) & ",'DO')"

                Try
                    ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'DO Insert :'," & SafeSQL(dtr("DocNum")) & "," & SafeSQL("") & ")")
                Catch ex As Exception

                End Try

                ExecuteSQL(sQrySQL)

               
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing


        If rs Is Nothing = False Then
            rs.Dispose()
            rs = Nothing
        End If

        ImportDOorderItem(arrList)



        '     ExecuteSQL("Update OrderHdr Set tempVehicleID=isnull(Customer.ShipAgent,'') from OrderHdr, Customer where Customer.CustNo=OrderHdr.CustId and isnull(tempVehicleID,'')=''")

        If bSync = True Then
            UpdateLastTimeStamp("DeliveryOrder", iValue)
        Else
            InsertLastTimeStamp("DeliveryOrder", iValue)
        End If

    End Sub


    Public Sub ImportSalesorder()
        Dim dtr As SqlDataReader
        Dim rs As SqlDataReader
        Dim dNewRecord As Int16 = 0
        Dim iValue As Date = Date.Now
        Dim ivalueLast30 As Date = iValue '.AddDays(-1)
        Dim bSync As Boolean = GetLastTimeStamp("Salesorder", iValue, dNewRecord)
        Dim arrList As New ArrayList
        Dim aord As New Arrord
        Dim sSQL As String = ""

        If bDeleteIns = True Then
            sSQL = "Select Distinct U_A_TRK from ORDR where DocStatus='O' "
            dtr = ReadNavRecord(sSQL)
            While dtr.Read
                If dtr("U_A_TRK").ToString <> "" Then
                    If IsExists("Select Code from Vehicle where code=" & SafeSQL(dtr("U_A_TRK"))) = False Then
                        ExecuteSQLAnother("Insert into Vehicle(Code, Description, Tonnage, Length, Width, Height, ToPDA, DisplayNum) Values (" & SafeSQL(dtr("U_A_TRK").ToString) & "," & SafeSQL(IIf(dtr("U_A_TRK").ToString = "", dtr("U_A_TRK").ToString, dtr("U_A_TRK").ToString)) & ", 5000, 1000,1000,1000,1,999)")
                    End If
                End If
            End While
            dtr.Close()
            dtr.Dispose()
        End If




        If bDeleteIns = True Then
            sSQL = "Select * from ORDR where DocStatus='O'  order by UpdateDate"
            'ExecuteSQL("Update OrdItem Set Qty=0")
        Else
            sSQL = "Select * from ORDR where  (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & " or CreateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & ") order by DocNum Desc"
        End If


        'System.IO.File.AppendAllText("C:\ImportLog.txt", "SO : " & Date.Now.ToString & vbCrLf & sSQL & vbCrLf)

        dtr = ReadNavRecord(sSQL)
        While dtr.Read
            Dim dDisAmt As Double = 0
            rs = ReadRecord("Select ordno from orderhdr where ordno= " & SafeSQL(dtr("DocNum")))

            If rs.Read = True Then

                aord.Ordno = dtr("DocNum").ToString
                aord.DocEntry = dtr("DocEntry").ToString
                arrList.Add(aord)
                rs.Close()

                If dtr("DocStatus") = "C" Then
                    ExecuteSQL("Update orderhdr set Void=1 where ordno =" & SafeSQL(dtr("DocNum")))
                    Try
                        ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'SO Cancel :'," & SafeSQL(dtr("DocNum")) & "," & SafeSQL("") & ")")
                    Catch ex As Exception

                    End Try

                Else
                    Try
                        ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'SO Update :'," & SafeSQL(dtr("DocNum")) & "," & SafeSQL("") & ")")
                    Catch ex As Exception

                    End Try
                    ExecuteSQL("Update orderhdr set OrdDt=" & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & ", tempVehicleID=" & SafeSQL(dtr("U_A_TRK").ToString) & ", Remarks=" & SafeSQL(dtr("CardCode").ToString) & ", DeliveryDate=" & SafeSQL(Format(dtr("ReqDate"), "yyyyMMdd HH:mm:ss")) & ", Custid = " & SafeSQL(dtr("cardcode")) & ", DTG = " & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & " where ordno =" & SafeSQL(dtr("DocNum")))
                End If

            Else

                aord.Ordno = dtr("DocNum").ToString
                aord.DocEntry = dtr("DocEntry").ToString
                arrList.Add(aord)
                rs.Close()
                ExecuteSQL("Insert into OrderHdr (OrdNo, OrdDt, CustId, PONo, AgentId, Discount, SubTotal, GSTAmt, TotalAmt, PayTerms, CurCode, CurExRate,Delivered,void,exported,DeliveryDate, DTG, Remarks, IsConfirmed, tempVehicleID, Attn) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("cardcode").ToString) & "," & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(dtr("Ref1").ToString) & "," & SafeSQL(0) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & "," & SafeSQL(dtr("SlpCode").ToString) & ", 0, " & "1,0,0,0" & "," & SafeSQL(Format(dtr("ReqDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(Format(Date.Now, "yyyyMMdd 00:00:00")).ToString & "," & SafeSQL(dtr("CardCode").ToString) & " ,1, " & SafeSQL(dtr("U_A_TRK").ToString) & ",'SO')")
                Try
                    ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'SO Insert :'," & SafeSQL(dtr("DocNum")) & "," & SafeSQL("") & ")")
                Catch ex As Exception

                End Try
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing


        If rs Is Nothing = False Then
            rs.Dispose()
            rs = Nothing
        End If

        ImportordItem(arrList)



        '     ExecuteSQL("Update OrderHdr Set tempVehicleID=isnull(Customer.ShipAgent,'') from OrderHdr, Customer where Customer.CustNo=OrderHdr.CustId and isnull(tempVehicleID,'')=''")

        If bSync = True Then
            UpdateLastTimeStamp("Salesorder", iValue)
        Else
            InsertLastTimeStamp("Salesorder", iValue)
        End If

    End Sub

    Public Sub Importtransferdet(ByVal arrList As ArrayList)

        Dim dtr As SqlDataReader
        For iIndex As Integer = 0 To arrList.Count - 1
            Dim aPo As ArrTrans
            aPo = arrList(iIndex)
            ExecuteSQL("Delete from transferdet where transno= " & SafeSQL(aPo.transno))

            dtr = ReadNavRecord("Select * from WTR1 where DocEntry = " & SafeSQL(aPo.DocEntry) & " order by LineNum")
            While dtr.Read
                
                If dtr("DocEntry").ToString = aPo.DocEntry Then
                    ExecuteSQL("Insert into TransferDet(TransNo, [LineNo], [AttachedToLineNo], ItemNo, Description, UOM, Qty, UnitPrice, ShippedQty, ReceivedQty, ShipmentDate, ReceivedDate, VariantCode) Values (" & SafeSQL(aPo.transno.ToString) & ", " & dtr("LineNum").ToString & ", " & 1 & "," & SafeSQL(dtr("ItemCode").ToString) & "," & SafeSQL(dtr("Dscription").ToString) & ", " & SafeSQL(dtr("UomEntry").ToString) & ", " & SafeSQL(dtr("Quantity").ToString) & ", " & 0 & ", " & SafeSQL(dtr("Quantity").ToString) & ", " & SafeSQL(dtr("Quantity").ToString) & ", " & SafeSQL("0") & ", " & SafeSQL("0") & ", " & SafeSQL(dtr("slpcode").ToString) & ")")
                End If
            End While
            dtr.Close()
        Next
        If dtr Is Nothing = False Then
            dtr.Dispose()
            dtr = Nothing
        End If
        ExecuteSQLAnother("Update orditem set UOM=Item.BaseUOM from orditem, Item where orditem.ItemNo= Item.ItemNo")
    End Sub



    Public Sub ImportDOorderItem(ByVal arrList As ArrayList)

        Dim sSQL As String = ""
        Dim dtr As SqlDataReader
        'System.IO.File.AppendAllText("C:\ImportLog.txt", "SO Det: " & Date.Now.ToString & vbCrLf & arrList.Count & vbCrLf)
        For iIndex As Integer = 0 To arrList.Count - 1
            Dim aPo As Arrord
            aPo = arrList(iIndex)
            ExecuteSQL("Update orditem Set Qty=0 where ordno= " & SafeSQL(aPo.Ordno))

            dtr = ReadNavRecord("Select * from DLN1 where DocEntry = " & SafeSQL(aPo.DocEntry) & " order by LineNum")
            While dtr.Read
                'MsgBox(dtr("DocEntry"))
                '  MsgBox(aPo.PONo)


                If dtr("DocEntry").ToString = aPo.DocEntry Then


                    If IsOrderItemExists(aPo.Ordno.ToString, dtr("Linenum")) = True Then

                        sSQL = "Update OrdItem Set ItemNo=" & SafeSQL(dtr("ItemCode").ToString) & ", Location=" & SafeSQL(dtr("WhsCode").ToString) & ", Description=" & SafeSQL(dtr("Dscription").ToString) & ", Qty =" & SafeSQL(dtr("InvQty").ToString) & "  where  OrdNo= " & SafeSQL(aPo.Ordno.ToString) & " and [LineNo]=" & dtr("Linenum")
                    Else
                        sSQL = "Insert into OrdItem (OrdNo, [LineNo],VariantCode,  ItemNo, Uom, Qty,Price, DisPr, DisPer, Discount, SubAmt, GSTAmt, DeliQty,FOC, Location, Description,DeliveryDate) Values (" & SafeSQL(aPo.Ordno.ToString) & ", " & SafeSQL(dtr("Linenum").ToString) & ", " & SafeSQL("") & "," & SafeSQL(dtr("ItemCode").ToString) & ", " & SafeSQL("") & ", " & SafeSQL(dtr("InvQty").ToString) & ", 0, " & SafeSQL(dtr("DiscPrcnt").ToString) & ", " & SafeSQL(dtr("DiscPrcnt").ToString) & ", 0, " & SafeSQL("0") & ", " & SafeSQL("0").ToString & ", " & SafeSQL(dtr("DelivrdQty").ToString) & ", " & "1" & "," & SafeSQL(dtr("WhsCode").ToString) & "," & SafeSQL(dtr("Dscription").ToString) & "," & SafeSQL("") & ")"
                    End If

                    'System.IO.File.AppendAllText("C:\ImportLog.txt", "SO Det : " & Date.Now.ToString & vbCrLf & sSQL & vbCrLf)

                    Try
                        ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'DO Item Query :'," & SafeSQL(aPo.Ordno.ToString) & "," & SafeSQL(sSQL) & ")")
                    Catch ex As Exception

                    End Try

                    ExecuteSQL(sSQL)
                End If

            End While
            dtr.Close()
            ExecuteSQLAnother("Update orditem set UOM=Item.BaseUOM from orditem, Item where orditem.ItemNo= Item.ItemNo and OrdNo=" & SafeSQL(aPo.Ordno))

        Next

        If dtr Is Nothing = False Then
            dtr.Dispose()
            dtr = Nothing
        End If
    End Sub



    Public Sub ImportordItem(ByVal arrList As ArrayList)

        Dim sSQL As String = ""
        Dim dtr As SqlDataReader
        'System.IO.File.AppendAllText("C:\ImportLog.txt", "SO Det: " & Date.Now.ToString & vbCrLf & arrList.Count & vbCrLf)
        For iIndex As Integer = 0 To arrList.Count - 1
            Dim aPo As Arrord
            aPo = arrList(iIndex)
            ExecuteSQL("Update orditem Set Qty=0 where ordno= " & SafeSQL(aPo.Ordno))

            dtr = ReadNavRecord("Select * from RDR1 where DocEntry = " & SafeSQL(aPo.DocEntry) & " order by LineNum")
            While dtr.Read
                'MsgBox(dtr("DocEntry"))
                '  MsgBox(aPo.PONo)
                If dtr("DocEntry").ToString = aPo.DocEntry Then
                    If IsOrderItemExists(aPo.Ordno.ToString, dtr("Linenum")) = True Then

                        sSQL = "Update OrdItem Set ItemNo=" & SafeSQL(dtr("ItemCode").ToString) & ", Location=" & SafeSQL(dtr("WhsCode").ToString) & ", Description=" & SafeSQL(dtr("Dscription").ToString) & ", Qty =" & SafeSQL(dtr("InvQty").ToString) & "  where  OrdNo= " & SafeSQL(aPo.Ordno.ToString) & " and [LineNo]=" & dtr("Linenum")
                    Else
                        sSQL = "Insert into OrdItem (OrdNo, [LineNo],VariantCode,  ItemNo, Uom, Qty,Price, DisPr, DisPer, Discount, SubAmt, GSTAmt, DeliQty,FOC, Location, Description,DeliveryDate) Values (" & SafeSQL(aPo.Ordno.ToString) & ", " & SafeSQL(dtr("Linenum").ToString) & ", " & SafeSQL("") & "," & SafeSQL(dtr("ItemCode").ToString) & ", " & SafeSQL("") & ", " & SafeSQL(dtr("InvQty").ToString) & ", 0, " & SafeSQL(dtr("DiscPrcnt").ToString) & ", " & SafeSQL(dtr("DiscPrcnt").ToString) & ", 0, " & SafeSQL("0") & ", " & SafeSQL("0").ToString & ", " & SafeSQL(dtr("DelivrdQty").ToString) & ", " & "1" & "," & SafeSQL(dtr("WhsCode").ToString) & "," & SafeSQL(dtr("Dscription").ToString) & "," & SafeSQL("") & ")"
                    End If

                    'System.IO.File.AppendAllText("C:\ImportLog.txt", "SO Det : " & Date.Now.ToString & vbCrLf & sSQL & vbCrLf)

                    Try
                        ExecuteSQL("Insert into ErrorLog(DTG, FunctionName, CompanyName, ErrorText) values (GetDate(),'SO Item Query :'," & SafeSQL(aPo.Ordno.ToString) & "," & SafeSQL(sSQL) & ")")
                    Catch ex As Exception

                    End Try
                    ExecuteSQL(sSQL)
                End If

            End While
            dtr.Close()
            ExecuteSQL("Update orditem set UOM=Item.BaseUOM from orditem, Item where orditem.ItemNo= Item.ItemNo and OrdNo=" & SafeSQL(aPo.Ordno))

        Next

        If dtr Is Nothing = False Then
            dtr.Dispose()
            dtr = Nothing
        End If
    End Sub


    Public Sub ImportPurchasecreditnote()
        Dim dNewRecord As Int16 = 0
        Dim iValue As Date = Date.Now
        Dim ivalueLast30 As Date = iValue.AddDays(-200)
        Dim bSync As Boolean = GetLastTimeStamp("Purchasecreditnote", iValue, dNewRecord)
        Dim dtr As SqlDataReader
        Dim rs As SqlDataReader
        Dim arrList As New ArrayList
        If dNewRecord = 0 Then
            dtr = ReadNavRecord("Select * from ORPC where DocTotal > PaidToDate and (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        Else
            dtr = ReadNavRecord("Select * from ORPC where DocTotal > PaidToDate and (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        End If
        While dtr.Read
            Dim dDisAmt As Double = 0
            rs = ReadRecord("Select creditnoteNo from POCreditNote where CreditNoteNo= " & SafeSQL(dtr("DocNum")))
            ' ExecuteSQL("Delete POCreditNote where CreditNoteNo= " & SafeSQL(dtr("DocNum")))
            Dim apCr As New ArrPrCrNote
            apCr.CreditNoteNo = dtr("DocNum").ToString
            apCr.DocEntry = dtr("DocEntry").ToString
            'apCr.DeliQty = ""
            'apCr.FromBin = dtr("CardCode").ToString
            'apCr.FromLocation = dtr("SlpCode").ToString
            arrList.Add(apCr)
            rs.Close()
            'ExecuteSQL("Insert into CreditNote(CreditNoteNo, CreditDate, CustNo, GoodsReturnNo, SalesPersonCode, Discount, SubTotal, GST, TotalAmt, PaidAmt, Void, Exported,DTG) Values (" & SafeSQL(dtr("No_").ToString) & "," & SafeSQL(Format(aDocDate, "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("Sell-to Customer No_").ToString) & ",''," & SafeSQL(dtr("Salesperson Code").ToString) & "," & dDisAmt & "," & (dtr("Amount")) & "," & CStr(((-1 * dtr("Original Amt_ (LCY)")) - (dtr("Amount")))) & "," & -1 * dtr("Original Amt_ (LCY)") & "," & CStr(-1 * ((-1 * dtr("Original Amt_ (LCY)")) - (-1 * dtr("Remaining Amount")))) & ",0,1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
            'ExecuteSQL("Insert into CreditNote(CreditNoteNo, CreditDate, CustNo, GoodsReturnNo, SalesPersonCode, Discount, SubTotal, GST, TotalAmt, PaidAmt, Void, Exported,DTG, CompanyName) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("CardCode").ToString) & ",''," & SafeSQL(dtr("SlpCode").ToString) & "," & dDisAmt & "," & dtr("DocTotal") - dtr("VatSum") & "," & dtr("VatSum") & "," & dtr("DocTotal") & "," & dtr("PaidToDate") & ",0,1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & "," & SafeSQL("STD") & ")")
            '            ExecuteSQL("Insert into POCreditNote(CreditNoteNo, CreditDate, CustNo, SalesPersonCode, Discount, SubTotal, GST, TotalAmt, Void, Exported, LocationCode, DeliveryDate, CompanyName) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(dtr("DocDate")) & "," & SafeSQL(dtr("CardCode").ToString) & "," & SafeSQL("") & ", 0, 0, 0, 0,1,1," & SafeSQL("") & "," & SafeSQL(dtr("PaidToDate")) & "," & SafeSQL("") & ")")
            ExecuteSQL("Insert into POCreditNote(CreditNoteNo, CreditDate, CustNo, SalesPersonCode, Discount, SubTotal, GST, TotalAmt, Void, Exported, LocationCode,  CompanyName) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(dtr("DocDate")) & "," & SafeSQL(dtr("CardCode").ToString) & "," & SafeSQL("") & ", 0, 0, 0, 0,1,1," & SafeSQL("") & "," & SafeSQL("") & ")")
            'End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing

        If rs Is Nothing = False Then
            rs.Dispose()
            rs = Nothing
        End If
        ImportPOCrNoteItem(arrList)

        'For iIndex As Integer = 0 To arrList.Count - 1
        '    Dim aP As New ArrCrNote
        '    aP = arrList(iIndex)
        '    dtr = ReadNavRecord("Select sum(Amount) as Amount, sum(""Amount Including VAT"") as TotalAmt,sum(""Amount Including VAT"" - Amount) as Gst  from """ & NavCompanyName & "Sales Cr_Memo Line"" where ""Document No_"" = " & SafeSQL(aP.CrNo) & " ")
        '    'rs = ReadRecord("Select SUM(SubAmt) as Amount, sum(gstamt) as Gst from CreditNoteDet where CreditNoteNo= " & SafeSQL(aP.CrNo))
        '    If dtr.Read = True Then
        '        ExecuteSQLAnother("Update CreditNote set SubTotal = " & IIf(IsDBNull(dtr("Amount")), 0, dtr("Amount")) & ", " & _
        '                          "GST = " & IIf(IsDBNull(dtr("Gst")), 0, dtr("Gst")) & ", TotalAmt = " & IIf(IsDBNull(dtr("TotalAmt")), 0, dtr("TotalAmt")) & " where CreditNoteNo =" & SafeSQL(aP.CrNo))
        '    End If
        '    dtr.Close()
        'Next
        If bSync = True Then
            UpdateLastTimeStamp("Purchasecreditnote", iValue)
        Else
            InsertLastTimeStamp("Purchasecreditnote", iValue)
        End If
    End Sub



    Public Sub ImportPOCrNoteItem(ByVal arrList As ArrayList)

        Dim dtr As SqlDataReader
        For iIndex As Integer = 0 To arrList.Count - 1
            Dim aPo As ArrPrCrNote
            aPo = arrList(iIndex)
            ExecuteSQL("Delete from POCreditNoteDet where CreditNoteNo= " & SafeSQL(aPo.CreditNoteNo))

            dtr = ReadNavRecord("Select * from RPC1 where DocEntry = " & SafeSQL(aPo.DocEntry) & " order by LineNum")
            While dtr.Read
                If dtr("DocEntry").ToString = aPo.DocEntry Then
                    '  ExecuteSQL("Insert into OrdItem (OrdNo, [LineNo],VariantCode,  ItemNo, Uom, Qty,Price, DisPr, DisPer, Discount, SubAmt, GSTAmt, DeliQty,FOC, Location, Description,DeliveryDate) Values (" & SafeSQL(aPo.Ordno.ToString) & ", " & SafeSQL(dtr("Linenum").ToString) & ", " & SafeSQL(dtr("Flags").ToString) & "," & SafeSQL(dtr("ItemCode").ToString) & ", " & SafeSQL(dtr("UomEntry").ToString) & ", " & SafeSQL(dtr("Quantity").ToString) & ", 0, " & SafeSQL(dtr("DiscPrcnt").ToString) & ", " & SafeSQL(dtr("DiscPrcnt").ToString) & ", 0, " & SafeSQL("0") & ", " & SafeSQL("0").ToString & ", " & SafeSQL(dtr("DelivrdQty").ToString) & ", " & "1" & "," & SafeSQL(dtr("WhsCode").ToString) & "," & SafeSQL(dtr("Dscription").ToString) & "," & SafeSQL("") & ")")
                    ExecuteSQL("Insert into POCreditNoteDet(CreditNoteNo, [LineNo], [AttachedToLineNo], ItemNo, UOM, Qty, Price, Amt, VariantCode,Description, FromLocation, FromBin, DeliQty, Remarks) Values (" & SafeSQL(aPo.CreditNoteNo.ToString) & ", " & SafeSQL(dtr("Linenum").ToString) & ", " & SafeSQL(dtr("Flags").ToString) & "," & SafeSQL(dtr("ItemCode").ToString) & ", " & SafeSQL(dtr("UomEntry").ToString) & ", " & SafeSQL(dtr("Quantity").ToString) & ", 0, " & SafeSQL(dtr("DiscPrcnt").ToString) & "," & SafeSQL("0") & "," & SafeSQL(dtr("Dscription").ToString) & "," & SafeSQL(dtr("WhsCode").ToString) & "," & SafeSQL("") & ",0," & SafeSQL(dtr("Dscription").ToString) & ")")
                End If
            End While
            dtr.Close()
        Next
        If dtr Is Nothing = False Then
            dtr.Dispose()
            dtr = Nothing
        End If
        ExecuteSQLAnother("Update orditem set UOM=Item.BaseUOM from orditem, Item where orditem.ItemNo= Item.ItemNo")
    End Sub



    Public Function GetShipToCode(ByVal scustno As String, ByVal sShipToName As String) As String
        Dim dtr As SqlDataReader
        Dim sShipToCode As String = ""
        'System.IO.File.AppendAllText("C:\ShipToCode.txt", "Select * from CustomerBill where CustNo = " & SafeSQL(scustno) & " and Address = " & SafeSQL(sShipToName))
        dtr = ReadRecord("Select * from CustomerBill where CustNo = " & SafeSQL(scustno) & " and Address = " & SafeSQL(sShipToName))
        If dtr.Read = True Then
            sShipToCode = dtr("AcBillRef").ToString
        End If
        dtr.Close()
        Return sShipToCode
    End Function


    Public Sub ImportCreditNote()
        Dim dNewRecord As Int16 = 0
        Dim iValue As Date = Date.Now
        Dim ivalueLast30 As Date = iValue.AddDays(-30)
        Dim bSync As Boolean = GetLastTimeStamp("CreditNote", iValue, dNewRecord)
        Dim dtr As SqlDataReader
        Dim rs As SqlDataReader
        Dim arrList As New ArrayList
        If dNewRecord = 0 Then
            ' dtr = ReadNavRecord("SELECT * FROM ORIN where DocTotal > PaidToDate and (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "YYYYMMdd")))
            dtr = ReadNavRecord("Select * from ORIN where DocTotal > PaidToDate and (UpdateDate >= " & SafeSQL(Format(ivalueLast30, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        Else
            dtr = ReadNavRecord("Select * from ORIN where DocTotal > PaidToDate and (UpdateDate >= " & SafeSQL(Format(iValue, "yyyyMMdd")) & " or UpdateDate is null) order by UpdateDate")
        End If
        While dtr.Read
            Dim dDisAmt As Double = 0
            rs = ReadRecord("Select PaidAmt from CreditNote where CreditNoteNo= " & SafeSQL(dtr("DocNum")))
            If rs.Read = True Then
                rs.Close()
                'ExecuteSQL("Update CreditNote set PaidAmt = " & -1 * (dtr("Original Amt_ (LCY)") - dtr("Rem_ Amt")) & " where CreditNoteNo =" & SafeSQL(dtr("No_")))
                ExecuteSQL("Update CreditNote set PaidAmt = " & dtr("PaidToDate") & " where CreditNoteNo =" & SafeSQL(dtr("DocNum")))
            Else
                Dim aCr As New ArrCrNote
                aCr.CrNo = dtr("DocNum").ToString
                aCr.CrDate = dtr("DocDate")
                aCr.OrdNo = ""
                aCr.CustID = dtr("CardCode").ToString
                aCr.AgentID = dtr("SlpCode").ToString
                aCr.Discount = dDisAmt
                aCr.CurCode = dtr("DocCur").ToString
                aCr.CurExRate = dtr("DocRate")
                aCr.GSTAmt = dtr("VatSum")
                aCr.Subtotal = dtr("DocTotal") - dtr("VatSum")
                aCr.TotalAmt = dtr("DocTotal")
                aCr.PaidAmt = dtr("PaidToDate")
                aCr.payterms = dtr("GroupNum").ToString
                arrList.Add(aCr)
                rs.Close()
                'ExecuteSQL("Insert into CreditNote(CreditNoteNo, CreditDate, CustNo, GoodsReturnNo, SalesPersonCode, Discount, SubTotal, GST, TotalAmt, PaidAmt, Void, Exported,DTG) Values (" & SafeSQL(dtr("No_").ToString) & "," & SafeSQL(Format(aDocDate, "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("Sell-to Customer No_").ToString) & ",''," & SafeSQL(dtr("Salesperson Code").ToString) & "," & dDisAmt & "," & (dtr("Amount")) & "," & CStr(((-1 * dtr("Original Amt_ (LCY)")) - (dtr("Amount")))) & "," & -1 * dtr("Original Amt_ (LCY)") & "," & CStr(-1 * ((-1 * dtr("Original Amt_ (LCY)")) - (-1 * dtr("Remaining Amount")))) & ",0,1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & ")")
                ExecuteSQL("Insert into CreditNote(CreditNoteNo, CreditDate, CustNo, GoodsReturnNo, SalesPersonCode, Discount, SubTotal, GST, TotalAmt, PaidAmt, Void, Exported,DTG, CompanyName) Values (" & SafeSQL(dtr("DocNum").ToString) & "," & SafeSQL(Format(dtr("DocDate"), "yyyyMMdd HH:mm:ss")) & "," & SafeSQL(dtr("CardCode").ToString) & ",''," & SafeSQL(dtr("SlpCode").ToString) & "," & dDisAmt & "," & dtr("DocTotal") - dtr("VatSum") & "," & dtr("VatSum") & "," & dtr("DocTotal") & "," & dtr("PaidToDate") & ",0,1," & SafeSQL(Format(Date.Now, "yyyyMMdd HH:mm:ss")) & "," & SafeSQL("STD") & ")")
            End If
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing

        If rs Is Nothing = False Then
            rs.Dispose()
            rs = Nothing
        End If
        '  ImportCrNoteItem(arrList)

        'For iIndex As Integer = 0 To arrList.Count - 1
        '    Dim aP As New ArrCrNote
        '    aP = arrList(iIndex)
        '    dtr = ReadNavRecord("Select sum(Amount) as Amount, sum(""Amount Including VAT"") as TotalAmt,sum(""Amount Including VAT"" - Amount) as Gst  from """ & NavCompanyName & "Sales Cr_Memo Line"" where ""Document No_"" = " & SafeSQL(aP.CrNo) & " ")
        '    'rs = ReadRecord("Select SUM(SubAmt) as Amount, sum(gstamt) as Gst from CreditNoteDet where CreditNoteNo= " & SafeSQL(aP.CrNo))
        '    If dtr.Read = True Then
        '        ExecuteSQLAnother("Update CreditNote set SubTotal = " & IIf(IsDBNull(dtr("Amount")), 0, dtr("Amount")) & ", " & _
        '                          "GST = " & IIf(IsDBNull(dtr("Gst")), 0, dtr("Gst")) & ", TotalAmt = " & IIf(IsDBNull(dtr("TotalAmt")), 0, dtr("TotalAmt")) & " where CreditNoteNo =" & SafeSQL(aP.CrNo))
        '    End If
        '    dtr.Close()
        'Next
        If bSync = True Then
            UpdateLastTimeStamp("CreditNote", iValue)
        Else
            InsertLastTimeStamp("CreditNote", iValue)
        End If
    End Sub



    Public Sub ImportCrNoteItem(ByVal arrList As ArrayList)


        '        Dim dtr As SqlDataReader
        '        For iIndex As Integer = 0 To arrList.Count - 1
        '            Dim aPr As ArrCrNote
        '            Dim CNT As Integer = 0
        '            aPr = arrList(iIndex)
        '            dtr = ReadNavRecord("Select * from """ & NavCompanyName & "Sales Cr_Memo Line"" where ""Document No_"" = " & SafeSQL(aPr.CrNo) & " order by ""Line No_""")
        '            While dtr.Read
        '                Try
        '                    CNT = 0
        'INSERT:
        '                    '              ExecuteSQL("Insert into InvItem(InvNo, [LineNo], ItemNo, UOM, Qty, Foc, Price, DisPer, DisPr, Discount, SubAmt, GstAmt, DeliQty, BaseUOM, BaseQty, Description) Values (" & SafeSQL(dtr("Document No_").ToString) & ", " & dtr("Line No_").ToString & ", " & SafeSQL(dtr("No_").ToString) & ", " & SafeSQL(dtr("Unit of Measure Code").ToString) & ", " & dtr("Quantity").ToString & ", 0, " & dtr("Unit Price").ToString & ", " & dtr("Line Discount %").ToString & ", 0, " & dtr("Line Discount Amount") & ", " & dtr("Amount").ToString & ", " & dtr("Amount Including GST") - dtr("Amount") & ", " & dtr("Quantity").ToString & ", " & SafeSQL(dtr("Unit of Measure Code").ToString) & ", " & dtr("Quantity (Base)").ToString & "," & SafeSQL(dtr("Description").ToString) & ")")
        '                    ExecuteSQL("Insert into CreditNoteDet(CreditNoteNo, ItemNo, UOM, BaseUOM, Price, Qty, Amt) Values (" & SafeSQL(dtr("Document No_").ToString) & ", " & SafeSQL(dtr("No_").ToString) & ", " & SafeSQL(dtr("Unit of Measure Code").ToString) & ",'', " & dtr("Unit Price").ToString & "," & dtr("Quantity").ToString & "," & dtr("Amount").ToString & ")")
        '                Catch
        '                    System.Threading.Thread.Sleep(5000)
        '                    CNT += 1
        '                    If CNT < 3 Then GoTo iNSERT
        '                    '       MsgBox(aPr.InvNo)
        '                End Try

        '            End While
        '            dtr.Close()
        '        Next

    End Sub

    Public Sub ImportInventory()
        Dim dtr As SqlDataReader
        ExecuteSQLAnother("Delete from GoodsInvn")
        dtr = ReadNavRecord("Select [WhsCode], [ItemCode], Sum([OnHand]) as Qty from OITW Where OnHand > 0 group by [WhsCode], [ItemCode]")
        While dtr.Read
            ExecuteSQLAnother("Insert into GoodsInvn(Location, ItemNo, Qty, UOM) Values (" & SafeSQL(dtr("WhsCode").ToString) & "," & SafeSQL(dtr("ItemCode").ToString) & "," & dtr("Qty") & ",'')")
        End While
        dtr.Close()
        dtr.Dispose()
        dtr = Nothing
        ExecuteSQLAnother("Update GoodsInvn set UOM=Item.BaseUOM from GoodsInvn, Item where GoodsInvn.ItemNo= Item.ItemNo")
    End Sub

    Private Function IsItemExists(ByVal sItemNo As String) As Boolean
        Dim dtr As SqlDataReader
        Dim bAns As Boolean
        dtr = ReadRecord("Select ItemNo from Item where ItemNo = " & SafeSQL(sItemNo))
        bAns = dtr.Read
        dtr.Close()
        Return bAns
    End Function
    Private Function IsItemPrExists(ByVal sItemNo As String, ByVal sPrGroup As String, ByVal dMinQty As Double, ByVal sSDate As Date, ByVal sEDate As Date) As Boolean
        Dim dtr As SqlDataReader
        Dim bAns As Boolean
        dtr = ReadRecordAnother("Select PriceGroup, ItemNo, MinQty from ItemPr where ItemNo = " & SafeSQL(sItemNo) & " and PriceGroup = " & SafeSQL(sPrGroup) & " and MinQty = " & SafeSQL(dMinQty) & " and FromDate = " & sSDate & " and ToDate = " & sEDate)
        If dtr.Read = True Then
            bAns = True
        Else
            bAns = False
        End If
        dtr.Close()
        Return bAns
    End Function

    Private Function IsAgentExists(ByVal sCode As String) As Boolean
        Dim dtr As SqlDataReader
        Dim bAns As Boolean
        dtr = ReadRecord("Select Code from SalesAgent where Code = " & SafeSQL(sCode))
        bAns = dtr.Read
        dtr.Close()
        Return bAns
    End Function

    Private Function IsCustomerExists(ByVal sCustNo As String) As Boolean
        Dim dtr As SqlDataReader
        Dim bAns As Boolean
        dtr = ReadRecord("Select CustNo from Customer where CustNo = " & SafeSQL(sCustNo))
        bAns = dtr.Read
        dtr.Close()
        Return bAns
    End Function

    Private Function IsPODetExists(ByVal sPONo As String, ByVal dLineNo As Integer) As Boolean
        Dim dtr As SqlDataReader
        Dim bAns As Boolean
        dtr = ReadRecord("Select * from PODet where PONo = " & SafeSQL(sPONo) & " and [LineNo]=" & dLineNo)
        bAns = dtr.Read
        dtr.Close()
        Return bAns
    End Function

    Private Function IsOrderItemExists(ByVal sOrdNo As String, ByVal dLineNo As Integer) As Boolean
        Dim dtr As SqlDataReader
        Dim bAns As Boolean
        dtr = ReadRecord("Select * from OrdItem where OrdNo = " & SafeSQL(sOrdNo) & " and [LineNo]=" & dLineNo)
        bAns = dtr.Read
        dtr.Close()
        Return bAns
    End Function

    Private Function IsVendorExists(ByVal sVendNo As String) As Boolean
        Dim dtr As SqlDataReader
        Dim bAns As Boolean
        dtr = ReadRecord("Select VendorNo from Vendor where VendorNo = " & SafeSQL(sVendNo))
        bAns = dtr.Read
        dtr.Close()
        Return bAns
    End Function

    Private Function IsNavCustomerExists(ByVal sCustNo As String) As Boolean
        Dim dtr As SqlDataReader
        Dim bAns As Boolean
        dtr = ReadNavRecord("Select * from Customer where No_ = " & SafeSQL(sCustNo))
        bAns = dtr.Read
        dtr.Close()
        Return bAns
    End Function

    Private Function UpdateCustName(ByVal sCustNo As String) As Integer
        Dim dtr As SqlDataReader
        Dim iAns As Integer = 0
        dtr = ReadRecord("Select CustName from Customer where AcCustCode = " & SafeSQL(sCustNo))
        If dtr.Read = True Then
            If dtr("CustName") = "" Then
                iAns = 1
            End If
        End If
        dtr.Close()
        Return iAns
    End Function

    Private Function HasBranch(ByVal sCustNo As String) As Integer
        Dim dtr As SqlDataReader
        Dim iAns As Integer = 0
        dtr = ReadRecord("Select CustNo from Customer where AcCustCode = " & SafeSQL(sCustNo))
        If dtr.Read = True Then
            If dtr("CustNo") = sCustNo Then
                iAns = 1
            Else
                iAns = 2
            End If
        End If
        dtr.Close()
        Return iAns
    End Function
    Private Sub UpdateCustomer()
        Dim dtr As SqlDataReader
        dtr = ReadRecord("Select Distinct AcCustCode, PriceGroup, PaymentTerms from Customer")
        While dtr.Read
            If IsDBNull(dtr("AcCustCode")) = False Then
                If dtr("AcCustCode").ToString <> "" Then
                    ExecuteSQLAnother("Update Customer Set PriceGroup =" & SafeSQL(dtr("PriceGroup").ToString) & ", PaymentTerms=" & SafeSQL(dtr("PaymentTerms").ToString) & " where Accustcode=" & SafeSQL(dtr("Accustcode").ToString))
                End If
            End If
        End While
        dtr.Close()
        ExecuteSQLAnother("Update Customer Set PriceGroup ='STD' Where (PriceGroup='' or PriceGroup is Null)")
    End Sub

    Public Sub ImportInvItem(ByVal arrList As ArrayList)
        'Dim sInvNos As String = "''"
        'For idx As Integer = 0 To arrList.Count - 1
        '    Dim aPr As ArrInvoice
        '    aPr = arrList(idx)
        '    sInvNos &= "," & SafeSQL(aPr.InvNo)
        'Next

        Dim dtr As SqlDataReader

        ' ExecuteSQL("Delete from InvItem where InvNo in (" & sInvNos & ")")

        For iIndex As Integer = 0 To arrList.Count - 1
            Dim aPr As ArrInvoice
            aPr = arrList(iIndex)
            ExecuteSQL("Delete from InvItem where InvNo= " & SafeSQL(aPr.InvNo))

            dtr = ReadNavRecord("Select * from OINM where Ref1 = " & SafeSQL(aPr.InvNo) & " order by Ref1")
            While dtr.Read
                'If dtr("Document No_").ToString = aPr.InvNo Then
                ExecuteSQL("Insert into InvItem(InvNo, [LineNo], ItemNo, UOM, Qty, Foc, Price, DisPer, DisPr, Discount, SubAmt, GstAmt, DeliQty, BaseUOM, BaseQty, Description, ColorRemarks) Values (" & SafeSQL(dtr("Ref1").ToString) & ", " & dtr("DocLineNum").ToString & ", " & SafeSQL(dtr("ItemCode").ToString) & ", " & SafeSQL("") & ", " & dtr("OutQty").ToString & ", 0, " & dtr("Price").ToString & ", 0, 0, 0, " & dtr("OutQty").ToString * dtr("Price").ToString & ", 0, " & dtr("OutQty").ToString & ", " & SafeSQL("") & ", 1," & SafeSQL(dtr("Dscription").ToString) & ",'')")
                'Exit For
                'End If
            End While
            dtr.Close()
        Next
        If dtr Is Nothing = False Then
            dtr.Dispose()
            dtr = Nothing
            ExecuteSQLAnother("Update InvItem set UOM=Item.BaseUOM from InvItem, Item where InvItem.ItemNo= Item.ItemNo")
        End If
    End Sub


    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click

    End Sub


    Public Sub loadCombo()
        Dim dtr As SqlDataReader
        dtr = ReadRecord("Select Code, Name from SalesAgent, MDT where MDT.AgentID=SalesAgent.Code")
        cmbAgent.DataSource = Nothing
        aAgent.Clear()
        aAgent.Add(New ComboValues("ALL", "ALL"))
        While dtr.Read()
            aAgent.Add(New ComboValues(dtr("Code").ToString, dtr("Name").ToString))
            '    iSelIndex = iIndex
            'End If
            'iIndex = iIndex + 1
        End While
        dtr.Close()
        cmbAgent.DataSource = aAgent
        cmbAgent.DisplayMember = "Desc"
        cmbAgent.ValueMember = "Code"
        cmbAgent.SelectedIndex = 0
    End Sub


    Private Sub chkSelAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelAll.CheckedChanged
        Dim i As Integer
        If chkSelAll.Checked = True Then
            For i = 0 To dgvStatus.Rows.Count - 1
                If dgvStatus.Rows(i).IsNewRow = True Then Exit For
                dgvStatus.Item(1, i).Value = True
            Next
        Else
            For i = 0 To dgvStatus.Rows.Count - 1
                If dgvStatus.Rows(i).IsNewRow = True Then Exit For
                dgvStatus.Item(1, i).Value = False
            Next
        End If
    End Sub

    End Class