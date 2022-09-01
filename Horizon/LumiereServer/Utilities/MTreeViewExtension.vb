Imports System.Runtime.CompilerServices


'' Source: https://pradeep1210.wordpress.com/2010/03/13/search-for-a-node-in-treeview-control-by-its-full-path/
Public Module MTreeViewExtension

    <Extension()>
    Public Function AddTreeNode(ByVal parentNode As TreeNode, ByVal text As String) As TreeNode
        Dim tn As New TreeNode(text)
        Dim leveledPath As String = parentNode.Name & "\" & text

        tn.Name = leveledPath
        parentNode.Nodes.Add(tn)

        Return tn
    End Function

    <Extension()>
    Public Function AddTreeNode(ByVal treeView As TreeView, ByVal text As String) As TreeNode
        Dim tn As New TreeNode(text)

        tn.Name = text
        treeView.Nodes.Add(tn)

        Return tn
    End Function

    <Extension()>
    Public Function GetNodeByFullPath(ByVal treeView As TreeView, ByVal fullPath As String) As TreeNode
        Dim tn() As TreeNode = treeView.Nodes.Find(fullPath, True)

        If tn.Count > 0 Then Return tn(0) Else Return Nothing
    End Function

    <Extension()>
    Public Function RegeditGetFullPath(ByVal node As TreeNode) As String
        Return node.Name
    End Function

End Module
