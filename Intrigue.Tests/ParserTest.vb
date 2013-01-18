
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue

<TestClass()> Public Class ParserTest

    <TestMethod()> Public Sub ParserNode_ToString()

        Assert.AreEqual(
            "1",
            (New Intrigue.Parser.Node(1)).ToString)
        Assert.AreEqual(
            "(1 2 3)",
            (New Intrigue.Parser.Node(1, 2, 3)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse()


        Assert.AreEqual(
            New Object() {},
            Intrigue.Parser.Parse("(+ 1 2 3)"))
    End Sub
End Class
