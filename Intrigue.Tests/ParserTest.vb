
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue


<TestClass()> Public Class ParserTest

    <TestMethod()> Public Sub ParserNode_ToString()

        Assert.AreEqual(
            "1",
            (New Intrigue.Node(1)).ToString)
        Assert.AreEqual(
            "(1 2 3)",
            (New Intrigue.Node(1, 2, 3)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_strings()

        Assert.AreEqual(
            "(xxx)",
            Intrigue.Parser.Parse("""xxx""").ToString)
        Assert.AreEqual(
            "(""x\""xx"")",
            Intrigue.Parser.Parse("""x\""xx""").ToString)
        Assert.AreEqual(
            "(""ab cd"")",
            Intrigue.Parser.Parse("""ab cd""").ToString)
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

    <TestMethod()> Public Sub Parser_Parse_lists()

        Assert.AreEqual(
            "(1 2 3)",
            Intrigue.Parser.Parse("(1 2 3)").ToString)
        Assert.AreEqual(
            "(1 ""nada niente"" 3)",
            Intrigue.Parser.Parse("(1 ""nada niente"" 3)").ToString)
        Assert.AreEqual(
            "(1 2 (3 4))",
            Intrigue.Parser.Parse("(1 2 (3 4))").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_symbol()

        Assert.AreEqual(
            "(alpha)",
            Intrigue.Parser.Parse("alpha").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_nobrackets()

        Assert.AreEqual(
            "(alpha 1 2 3)",
            Intrigue.Parser.Parse("alpha 1 2 3").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_multilines()

        Assert.AreEqual(
            "((alpha 1 2 3) (bravo 4 5 6))",
            Intrigue.Parser.Parse(
                <string>
                    (alpha 1 2 3)
                    (bravo 4 5 6)
                </string>.Nodes.First.ToString.Trim
            ).ToString)

        Assert.AreEqual(
            "((alpha 1 2 3) (bravo 4 5 6))",
            Intrigue.Parser.Parse(
                <string>
                    alpha 1 2 3
                    bravo 4 5 6
                </string>.Nodes.First.ToString.Trim
            ).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_quoted_list()

        Assert.AreEqual(
            "(quote 1 2 3)",
            Intrigue.Parser.Parse("(quote 1 2 3)").ToString)
        Assert.AreEqual(
            "(quote 1 2 3)",
            Intrigue.Parser.Parse("'(1 2 3)").ToString)
        Assert.AreEqual(
            "(quote 1 2 3)",
            Intrigue.Parser.Parse("quote 1 2 3").ToString)
    End Sub
End Class
