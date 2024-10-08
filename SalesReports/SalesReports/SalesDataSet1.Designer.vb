﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.42
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System


<System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0"),  _
 Serializable(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.ComponentModel.ToolboxItem(true),  _
 System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedDataSetSchema"),  _
 System.Xml.Serialization.XmlRootAttribute("SalesDataSet1"),  _
 System.ComponentModel.Design.HelpKeywordAttribute("vs.data.DataSet")>  _
Partial Public Class SalesDataSet1
    Inherits System.Data.DataSet
    
    Private tableSyncHistory As SyncHistoryDataTable
    
    Private _schemaSerializationMode As System.Data.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Sub New()
        MyBase.New
        Me.BeginInit
        Me.InitClass
        Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
        AddHandler MyBase.Tables.CollectionChanged, schemaChangedHandler
        AddHandler MyBase.Relations.CollectionChanged, schemaChangedHandler
        Me.EndInit
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context, false)
        If (Me.IsBinarySerialized(info, context) = true) Then
            Me.InitVars(false)
            Dim schemaChangedHandler1 As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
            AddHandler Me.Tables.CollectionChanged, schemaChangedHandler1
            AddHandler Me.Relations.CollectionChanged, schemaChangedHandler1
            Return
        End If
        Dim strSchema As String = CType(info.GetValue("XmlSchema", GetType(String)),String)
        If (Me.DetermineSchemaSerializationMode(info, context) = System.Data.SchemaSerializationMode.IncludeSchema) Then
            Dim ds As System.Data.DataSet = New System.Data.DataSet
            ds.ReadXmlSchema(New System.Xml.XmlTextReader(New System.IO.StringReader(strSchema)))
            If (Not (ds.Tables("SyncHistory")) Is Nothing) Then
                MyBase.Tables.Add(New SyncHistoryDataTable(ds.Tables("SyncHistory")))
            End If
            Me.DataSetName = ds.DataSetName
            Me.Prefix = ds.Prefix
            Me.Namespace = ds.Namespace
            Me.Locale = ds.Locale
            Me.CaseSensitive = ds.CaseSensitive
            Me.EnforceConstraints = ds.EnforceConstraints
            Me.Merge(ds, false, System.Data.MissingSchemaAction.Add)
            Me.InitVars
        Else
            Me.ReadXmlSchema(New System.Xml.XmlTextReader(New System.IO.StringReader(strSchema)))
        End If
        Me.GetSerializationData(info, context)
        Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
        AddHandler MyBase.Tables.CollectionChanged, schemaChangedHandler
        AddHandler Me.Relations.CollectionChanged, schemaChangedHandler
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.ComponentModel.Browsable(false),  _
     System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>  _
    Public ReadOnly Property SyncHistory() As SyncHistoryDataTable
        Get
            Return Me.tableSyncHistory
        End Get
    End Property
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.ComponentModel.BrowsableAttribute(true),  _
     System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Visible)>  _
    Public Overrides Property SchemaSerializationMode() As System.Data.SchemaSerializationMode
        Get
            Return Me._schemaSerializationMode
        End Get
        Set
            Me._schemaSerializationMode = value
        End Set
    End Property
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)>  _
    Public Shadows ReadOnly Property Tables() As System.Data.DataTableCollection
        Get
            Return MyBase.Tables
        End Get
    End Property
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)>  _
    Public Shadows ReadOnly Property Relations() As System.Data.DataRelationCollection
        Get
            Return MyBase.Relations
        End Get
    End Property
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Protected Overrides Sub InitializeDerivedDataSet()
        Me.BeginInit
        Me.InitClass
        Me.EndInit
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Overrides Function Clone() As System.Data.DataSet
        Dim cln As SalesDataSet1 = CType(MyBase.Clone,SalesDataSet1)
        cln.InitVars
        cln.SchemaSerializationMode = Me.SchemaSerializationMode
        Return cln
    End Function
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Protected Overrides Function ShouldSerializeTables() As Boolean
        Return false
    End Function
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Protected Overrides Function ShouldSerializeRelations() As Boolean
        Return false
    End Function
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Protected Overrides Sub ReadXmlSerializable(ByVal reader As System.Xml.XmlReader)
        If (Me.DetermineSchemaSerializationMode(reader) = System.Data.SchemaSerializationMode.IncludeSchema) Then
            Me.Reset
            Dim ds As System.Data.DataSet = New System.Data.DataSet
            ds.ReadXml(reader)
            If (Not (ds.Tables("SyncHistory")) Is Nothing) Then
                MyBase.Tables.Add(New SyncHistoryDataTable(ds.Tables("SyncHistory")))
            End If
            Me.DataSetName = ds.DataSetName
            Me.Prefix = ds.Prefix
            Me.Namespace = ds.Namespace
            Me.Locale = ds.Locale
            Me.CaseSensitive = ds.CaseSensitive
            Me.EnforceConstraints = ds.EnforceConstraints
            Me.Merge(ds, false, System.Data.MissingSchemaAction.Add)
            Me.InitVars
        Else
            Me.ReadXml(reader)
            Me.InitVars
        End If
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Protected Overrides Function GetSchemaSerializable() As System.Xml.Schema.XmlSchema
        Dim stream As System.IO.MemoryStream = New System.IO.MemoryStream
        Me.WriteXmlSchema(New System.Xml.XmlTextWriter(stream, Nothing))
        stream.Position = 0
        Return System.Xml.Schema.XmlSchema.Read(New System.Xml.XmlTextReader(stream), Nothing)
    End Function
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Friend Overloads Sub InitVars()
        Me.InitVars(true)
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Friend Overloads Sub InitVars(ByVal initTable As Boolean)
        Me.tableSyncHistory = CType(MyBase.Tables("SyncHistory"),SyncHistoryDataTable)
        If (initTable = true) Then
            If (Not (Me.tableSyncHistory) Is Nothing) Then
                Me.tableSyncHistory.InitVars
            End If
        End If
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Private Sub InitClass()
        Me.DataSetName = "SalesDataSet1"
        Me.Prefix = ""
        Me.Namespace = "http://tempuri.org/SalesDataSet1.xsd"
        Me.EnforceConstraints = true
        Me.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        Me.tableSyncHistory = New SyncHistoryDataTable
        MyBase.Tables.Add(Me.tableSyncHistory)
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Private Function ShouldSerializeSyncHistory() As Boolean
        Return false
    End Function
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Private Sub SchemaChanged(ByVal sender As Object, ByVal e As System.ComponentModel.CollectionChangeEventArgs)
        If (e.Action = System.ComponentModel.CollectionChangeAction.Remove) Then
            Me.InitVars
        End If
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Shared Function GetTypedDataSetSchema(ByVal xs As System.Xml.Schema.XmlSchemaSet) As System.Xml.Schema.XmlSchemaComplexType
        Dim ds As SalesDataSet1 = New SalesDataSet1
        Dim type As System.Xml.Schema.XmlSchemaComplexType = New System.Xml.Schema.XmlSchemaComplexType
        Dim sequence As System.Xml.Schema.XmlSchemaSequence = New System.Xml.Schema.XmlSchemaSequence
        xs.Add(ds.GetSchemaSerializable)
        Dim any As System.Xml.Schema.XmlSchemaAny = New System.Xml.Schema.XmlSchemaAny
        any.Namespace = ds.Namespace
        sequence.Items.Add(any)
        type.Particle = sequence
        Return type
    End Function
    
    Public Delegate Sub SyncHistoryRowChangeEventHandler(ByVal sender As Object, ByVal e As SyncHistoryRowChangeEvent)
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0"),  _
     System.Serializable(),  _
     System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")>  _
    Partial Public Class SyncHistoryDataTable
        Inherits System.Data.DataTable
        Implements System.Collections.IEnumerable
        
        Private columnAgentID As System.Data.DataColumn
        
        Private columnLocation As System.Data.DataColumn
        
        Private columnSyncTime As System.Data.DataColumn
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Sub New()
            MyBase.New
            Me.TableName = "SyncHistory"
            Me.BeginInit
            Me.InitClass
            Me.EndInit
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Friend Sub New(ByVal table As System.Data.DataTable)
            MyBase.New
            Me.TableName = table.TableName
            If (table.CaseSensitive <> table.DataSet.CaseSensitive) Then
                Me.CaseSensitive = table.CaseSensitive
            End If
            If (table.Locale.ToString <> table.DataSet.Locale.ToString) Then
                Me.Locale = table.Locale
            End If
            If (table.Namespace <> table.DataSet.Namespace) Then
                Me.Namespace = table.Namespace
            End If
            Me.Prefix = table.Prefix
            Me.MinimumCapacity = table.MinimumCapacity
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
            Me.InitVars
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public ReadOnly Property AgentIDColumn() As System.Data.DataColumn
            Get
                Return Me.columnAgentID
            End Get
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public ReadOnly Property LocationColumn() As System.Data.DataColumn
            Get
                Return Me.columnLocation
            End Get
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public ReadOnly Property SyncTimeColumn() As System.Data.DataColumn
            Get
                Return Me.columnSyncTime
            End Get
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         System.ComponentModel.Browsable(false)>  _
        Public ReadOnly Property Count() As Integer
            Get
                Return Me.Rows.Count
            End Get
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Default ReadOnly Property Item(ByVal index As Integer) As SyncHistoryRow
            Get
                Return CType(Me.Rows(index),SyncHistoryRow)
            End Get
        End Property
        
        Public Event SyncHistoryRowChanging As SyncHistoryRowChangeEventHandler
        
        Public Event SyncHistoryRowChanged As SyncHistoryRowChangeEventHandler
        
        Public Event SyncHistoryRowDeleting As SyncHistoryRowChangeEventHandler
        
        Public Event SyncHistoryRowDeleted As SyncHistoryRowChangeEventHandler
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Overloads Sub AddSyncHistoryRow(ByVal row As SyncHistoryRow)
            Me.Rows.Add(row)
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Overloads Function AddSyncHistoryRow(ByVal AgentID As String, ByVal Location As String, ByVal SyncTime As Date) As SyncHistoryRow
            Dim rowSyncHistoryRow As SyncHistoryRow = CType(Me.NewRow,SyncHistoryRow)
            rowSyncHistoryRow.ItemArray = New Object() {AgentID, Location, SyncTime}
            Me.Rows.Add(rowSyncHistoryRow)
            Return rowSyncHistoryRow
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Overridable Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return Me.Rows.GetEnumerator
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Overrides Function Clone() As System.Data.DataTable
            Dim cln As SyncHistoryDataTable = CType(MyBase.Clone,SyncHistoryDataTable)
            cln.InitVars
            Return cln
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Protected Overrides Function CreateInstance() As System.Data.DataTable
            Return New SyncHistoryDataTable
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Friend Sub InitVars()
            Me.columnAgentID = MyBase.Columns("AgentID")
            Me.columnLocation = MyBase.Columns("Location")
            Me.columnSyncTime = MyBase.Columns("SyncTime")
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Private Sub InitClass()
            Me.columnAgentID = New System.Data.DataColumn("AgentID", GetType(String), Nothing, System.Data.MappingType.Element)
            MyBase.Columns.Add(Me.columnAgentID)
            Me.columnLocation = New System.Data.DataColumn("Location", GetType(String), Nothing, System.Data.MappingType.Element)
            MyBase.Columns.Add(Me.columnLocation)
            Me.columnSyncTime = New System.Data.DataColumn("SyncTime", GetType(Date), Nothing, System.Data.MappingType.Element)
            MyBase.Columns.Add(Me.columnSyncTime)
            Me.columnAgentID.MaxLength = 20
            Me.columnLocation.MaxLength = 20
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Function NewSyncHistoryRow() As SyncHistoryRow
            Return CType(Me.NewRow,SyncHistoryRow)
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Protected Overrides Function NewRowFromBuilder(ByVal builder As System.Data.DataRowBuilder) As System.Data.DataRow
            Return New SyncHistoryRow(builder)
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Protected Overrides Function GetRowType() As System.Type
            Return GetType(SyncHistoryRow)
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Protected Overrides Sub OnRowChanged(ByVal e As System.Data.DataRowChangeEventArgs)
            MyBase.OnRowChanged(e)
            If (Not (Me.SyncHistoryRowChangedEvent) Is Nothing) Then
                RaiseEvent SyncHistoryRowChanged(Me, New SyncHistoryRowChangeEvent(CType(e.Row,SyncHistoryRow), e.Action))
            End If
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Protected Overrides Sub OnRowChanging(ByVal e As System.Data.DataRowChangeEventArgs)
            MyBase.OnRowChanging(e)
            If (Not (Me.SyncHistoryRowChangingEvent) Is Nothing) Then
                RaiseEvent SyncHistoryRowChanging(Me, New SyncHistoryRowChangeEvent(CType(e.Row,SyncHistoryRow), e.Action))
            End If
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Protected Overrides Sub OnRowDeleted(ByVal e As System.Data.DataRowChangeEventArgs)
            MyBase.OnRowDeleted(e)
            If (Not (Me.SyncHistoryRowDeletedEvent) Is Nothing) Then
                RaiseEvent SyncHistoryRowDeleted(Me, New SyncHistoryRowChangeEvent(CType(e.Row,SyncHistoryRow), e.Action))
            End If
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Protected Overrides Sub OnRowDeleting(ByVal e As System.Data.DataRowChangeEventArgs)
            MyBase.OnRowDeleting(e)
            If (Not (Me.SyncHistoryRowDeletingEvent) Is Nothing) Then
                RaiseEvent SyncHistoryRowDeleting(Me, New SyncHistoryRowChangeEvent(CType(e.Row,SyncHistoryRow), e.Action))
            End If
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Sub RemoveSyncHistoryRow(ByVal row As SyncHistoryRow)
            Me.Rows.Remove(row)
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Shared Function GetTypedTableSchema(ByVal xs As System.Xml.Schema.XmlSchemaSet) As System.Xml.Schema.XmlSchemaComplexType
            Dim type As System.Xml.Schema.XmlSchemaComplexType = New System.Xml.Schema.XmlSchemaComplexType
            Dim sequence As System.Xml.Schema.XmlSchemaSequence = New System.Xml.Schema.XmlSchemaSequence
            Dim ds As SalesDataSet1 = New SalesDataSet1
            xs.Add(ds.GetSchemaSerializable)
            Dim any1 As System.Xml.Schema.XmlSchemaAny = New System.Xml.Schema.XmlSchemaAny
            any1.Namespace = "http://www.w3.org/2001/XMLSchema"
            any1.MinOccurs = New Decimal(0)
            any1.MaxOccurs = Decimal.MaxValue
            any1.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax
            sequence.Items.Add(any1)
            Dim any2 As System.Xml.Schema.XmlSchemaAny = New System.Xml.Schema.XmlSchemaAny
            any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1"
            any2.MinOccurs = New Decimal(1)
            any2.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax
            sequence.Items.Add(any2)
            Dim attribute1 As System.Xml.Schema.XmlSchemaAttribute = New System.Xml.Schema.XmlSchemaAttribute
            attribute1.Name = "namespace"
            attribute1.FixedValue = ds.Namespace
            type.Attributes.Add(attribute1)
            Dim attribute2 As System.Xml.Schema.XmlSchemaAttribute = New System.Xml.Schema.XmlSchemaAttribute
            attribute2.Name = "tableTypeName"
            attribute2.FixedValue = "SyncHistoryDataTable"
            type.Attributes.Add(attribute2)
            type.Particle = sequence
            Return type
        End Function
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")>  _
    Partial Public Class SyncHistoryRow
        Inherits System.Data.DataRow
        
        Private tableSyncHistory As SyncHistoryDataTable
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Friend Sub New(ByVal rb As System.Data.DataRowBuilder)
            MyBase.New(rb)
            Me.tableSyncHistory = CType(Me.Table,SyncHistoryDataTable)
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Property AgentID() As String
            Get
                Try 
                    Return CType(Me(Me.tableSyncHistory.AgentIDColumn),String)
                Catch e As System.InvalidCastException
                    Throw New System.Data.StrongTypingException("The value for column 'AgentID' in table 'SyncHistory' is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableSyncHistory.AgentIDColumn) = value
            End Set
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Property Location() As String
            Get
                Try 
                    Return CType(Me(Me.tableSyncHistory.LocationColumn),String)
                Catch e As System.InvalidCastException
                    Throw New System.Data.StrongTypingException("The value for column 'Location' in table 'SyncHistory' is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableSyncHistory.LocationColumn) = value
            End Set
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Property SyncTime() As Date
            Get
                Try 
                    Return CType(Me(Me.tableSyncHistory.SyncTimeColumn),Date)
                Catch e As System.InvalidCastException
                    Throw New System.Data.StrongTypingException("The value for column 'SyncTime' in table 'SyncHistory' is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableSyncHistory.SyncTimeColumn) = value
            End Set
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Function IsAgentIDNull() As Boolean
            Return Me.IsNull(Me.tableSyncHistory.AgentIDColumn)
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Sub SetAgentIDNull()
            Me(Me.tableSyncHistory.AgentIDColumn) = System.Convert.DBNull
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Function IsLocationNull() As Boolean
            Return Me.IsNull(Me.tableSyncHistory.LocationColumn)
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Sub SetLocationNull()
            Me(Me.tableSyncHistory.LocationColumn) = System.Convert.DBNull
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Function IsSyncTimeNull() As Boolean
            Return Me.IsNull(Me.tableSyncHistory.SyncTimeColumn)
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Sub SetSyncTimeNull()
            Me(Me.tableSyncHistory.SyncTimeColumn) = System.Convert.DBNull
        End Sub
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")>  _
    Public Class SyncHistoryRowChangeEvent
        Inherits System.EventArgs
        
        Private eventRow As SyncHistoryRow
        
        Private eventAction As System.Data.DataRowAction
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Sub New(ByVal row As SyncHistoryRow, ByVal action As System.Data.DataRowAction)
            MyBase.New
            Me.eventRow = row
            Me.eventAction = action
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public ReadOnly Property Row() As SyncHistoryRow
            Get
                Return Me.eventRow
            End Get
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public ReadOnly Property Action() As System.Data.DataRowAction
            Get
                Return Me.eventAction
            End Get
        End Property
    End Class
End Class

Namespace SalesDataSet1TableAdapters
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0"),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.ComponentModel.ToolboxItem(true),  _
     System.ComponentModel.DataObjectAttribute(true),  _
     System.ComponentModel.DesignerAttribute("Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner"& _ 
        ", Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),  _
     System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")>  _
    Partial Public Class SyncHistoryTableAdapter
        Inherits System.ComponentModel.Component
        
        Private WithEvents _adapter As System.Data.SqlClient.SqlDataAdapter
        
        Private _connection As System.Data.SqlClient.SqlConnection
        
        Private _commandCollection() As System.Data.SqlClient.SqlCommand
        
        Private _clearBeforeFill As Boolean
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Sub New()
            MyBase.New
            Me.ClearBeforeFill = true
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Private ReadOnly Property Adapter() As System.Data.SqlClient.SqlDataAdapter
            Get
                If (Me._adapter Is Nothing) Then
                    Me.InitAdapter
                End If
                Return Me._adapter
            End Get
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Friend Property Connection() As System.Data.SqlClient.SqlConnection
            Get
                If (Me._connection Is Nothing) Then
                    Me.InitConnection
                End If
                Return Me._connection
            End Get
            Set
                Me._connection = value
                If (Not (Me.Adapter.InsertCommand) Is Nothing) Then
                    Me.Adapter.InsertCommand.Connection = value
                End If
                If (Not (Me.Adapter.DeleteCommand) Is Nothing) Then
                    Me.Adapter.DeleteCommand.Connection = value
                End If
                If (Not (Me.Adapter.UpdateCommand) Is Nothing) Then
                    Me.Adapter.UpdateCommand.Connection = value
                End If
                Dim i As Integer = 0
                Do While (i < Me.CommandCollection.Length)
                    If (Not (Me.CommandCollection(i)) Is Nothing) Then
                        CType(Me.CommandCollection(i),System.Data.SqlClient.SqlCommand).Connection = value
                    End If
                    i = (i + 1)
                Loop
            End Set
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Protected ReadOnly Property CommandCollection() As System.Data.SqlClient.SqlCommand()
            Get
                If (Me._commandCollection Is Nothing) Then
                    Me.InitCommandCollection
                End If
                Return Me._commandCollection
            End Get
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Property ClearBeforeFill() As Boolean
            Get
                Return Me._clearBeforeFill
            End Get
            Set
                Me._clearBeforeFill = value
            End Set
        End Property
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Private Sub InitAdapter()
            Me._adapter = New System.Data.SqlClient.SqlDataAdapter
            Dim tableMapping As System.Data.Common.DataTableMapping = New System.Data.Common.DataTableMapping
            tableMapping.SourceTable = "Table"
            tableMapping.DataSetTable = "SyncHistory"
            tableMapping.ColumnMappings.Add("AgentID", "AgentID")
            tableMapping.ColumnMappings.Add("Location", "Location")
            tableMapping.ColumnMappings.Add("SyncTime", "SyncTime")
            Me._adapter.TableMappings.Add(tableMapping)
            Me._adapter.InsertCommand = New System.Data.SqlClient.SqlCommand
            Me._adapter.InsertCommand.Connection = Me.Connection
            Me._adapter.InsertCommand.CommandText = "INSERT INTO [dbo].[SyncHistory] ([SyncTime], [AgentID], [Location]) VALUES (@Sync"& _ 
                "Time, @AgentID, @Location)"
            Me._adapter.InsertCommand.CommandType = System.Data.CommandType.Text
            Me._adapter.InsertCommand.Parameters.Add(New System.Data.SqlClient.SqlParameter("@SyncTime", System.Data.SqlDbType.DateTime, 0, System.Data.ParameterDirection.Input, 0, 0, "SyncTime", System.Data.DataRowVersion.Current, false, Nothing, "", "", ""))
            Me._adapter.InsertCommand.Parameters.Add(New System.Data.SqlClient.SqlParameter("@AgentID", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, 0, 0, "AgentID", System.Data.DataRowVersion.Current, false, Nothing, "", "", ""))
            Me._adapter.InsertCommand.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Location", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, 0, 0, "Location", System.Data.DataRowVersion.Current, false, Nothing, "", "", ""))
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Private Sub InitConnection()
            Me._connection = New System.Data.SqlClient.SqlConnection
            Me._connection.ConnectionString = Global.SalesReports.My.MySettings.Default.SalesConnectionString
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Private Sub InitCommandCollection()
            Me._commandCollection = New System.Data.SqlClient.SqlCommand(0) {}
            Me._commandCollection(0) = New System.Data.SqlClient.SqlCommand
            Me._commandCollection(0).Connection = Me.Connection
            Me._commandCollection(0).CommandText = "SELECT SyncTime, AgentID, Location FROM dbo.SyncHistory order by SyncTime Desc"
            Me._commandCollection(0).CommandType = System.Data.CommandType.Text
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter"),  _
         System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Fill, true)>  _
        Public Overloads Overridable Function Fill(ByVal dataTable As SalesDataSet1.SyncHistoryDataTable) As Integer
            Me.Adapter.SelectCommand = Me.CommandCollection(0)
            If (Me.ClearBeforeFill = true) Then
                dataTable.Clear
            End If
            Dim returnValue As Integer = Me.Adapter.Fill(dataTable)
            Return returnValue
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter"),  _
         System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.[Select], true)>  _
        Public Overloads Overridable Function GetData() As SalesDataSet1.SyncHistoryDataTable
            Me.Adapter.SelectCommand = Me.CommandCollection(0)
            Dim dataTable As SalesDataSet1.SyncHistoryDataTable = New SalesDataSet1.SyncHistoryDataTable
            Me.Adapter.Fill(dataTable)
            Return dataTable
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")>  _
        Public Overloads Overridable Function Update(ByVal dataTable As SalesDataSet1.SyncHistoryDataTable) As Integer
            Return Me.Adapter.Update(dataTable)
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")>  _
        Public Overloads Overridable Function Update(ByVal dataSet As SalesDataSet1) As Integer
            Return Me.Adapter.Update(dataSet, "SyncHistory")
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")>  _
        Public Overloads Overridable Function Update(ByVal dataRow As System.Data.DataRow) As Integer
            Return Me.Adapter.Update(New System.Data.DataRow() {dataRow})
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")>  _
        Public Overloads Overridable Function Update(ByVal dataRows() As System.Data.DataRow) As Integer
            Return Me.Adapter.Update(dataRows)
        End Function
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter"),  _
         System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)>  _
        Public Overloads Overridable Function Insert(ByVal SyncTime As System.Nullable(Of Date), ByVal AgentID As String, ByVal Location As String) As Integer
            If (SyncTime.HasValue = true) Then
                Me.Adapter.InsertCommand.Parameters(0).Value = CType(SyncTime.Value,Date)
            Else
                Me.Adapter.InsertCommand.Parameters(0).Value = System.DBNull.Value
            End If
            If (AgentID Is Nothing) Then
                Me.Adapter.InsertCommand.Parameters(1).Value = System.DBNull.Value
            Else
                Me.Adapter.InsertCommand.Parameters(1).Value = CType(AgentID,String)
            End If
            If (Location Is Nothing) Then
                Me.Adapter.InsertCommand.Parameters(2).Value = System.DBNull.Value
            Else
                Me.Adapter.InsertCommand.Parameters(2).Value = CType(Location,String)
            End If
            Dim previousConnectionState As System.Data.ConnectionState = Me.Adapter.InsertCommand.Connection.State
            If ((Me.Adapter.InsertCommand.Connection.State And System.Data.ConnectionState.Open)  _
                        <> System.Data.ConnectionState.Open) Then
                Me.Adapter.InsertCommand.Connection.Open
            End If
            Try 
                Dim returnValue As Integer = Me.Adapter.InsertCommand.ExecuteNonQuery
                Return returnValue
            Finally
                If (previousConnectionState = System.Data.ConnectionState.Closed) Then
                    Me.Adapter.InsertCommand.Connection.Close
                End If
            End Try
        End Function
    End Class
End Namespace
