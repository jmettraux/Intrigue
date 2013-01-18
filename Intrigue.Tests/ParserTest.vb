
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

    <TestMethod()> Public Sub Parser_Parse_strings()

        Assert.AreEqual(
            "(""xxx"")",
            Intrigue.Parser.Parse("""xxx""").ToString)
        Assert.AreEqual(
            "(""x\""xx"")",
            Intrigue.Parser.Parse("""x\""xx""").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_integers()

        Assert.AreEqual(
            "(1)",
            Intrigue.Parser.Parse("1").ToString)
        Assert.AreEqual(
            "(12)",
            Intrigue.Parser.Parse("12").ToString)
        Assert.AreEqual(
            "(-12)",
            Intrigue.Parser.Parse("-12").ToString)
    End Sub
End Class
