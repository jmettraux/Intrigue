
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue

<TestClass()> Public Class ParserTest

    <TestMethod()> Public Sub Parser_Stringify()

        Assert.AreEqual(
            "()",
            Parser.Stringify(New Object() {}))
        Assert.AreEqual(
            "(1 2)",
            Parser.Stringify(New Object() {1, 2}))
    End Sub

    <TestMethod()> Public Sub Parser_Parse()

        Assert.AreEqual(
            New Object() {},
            Intrigue.Parser.Parse("(+ 1 2 3)"))
    End Sub
End Class
