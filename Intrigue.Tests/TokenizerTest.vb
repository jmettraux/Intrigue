
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue
Imports Intrigue.Parsing


<TestClass()> Public Class TokenizerTest

    <TestMethod()> Public Sub Tokenizer_Tokenize()

        Dim ts = Intrigue.Parsing.Tokenizer.Tokenize(
            Intrigue.Util.NewString(
                <string>
                   (alpha 1 2 3) ; nada
                   ; nada
                   bravo 4 5 6 ; nada
                   "hello"
               </string>))

        'Console.WriteLine(Tokenizer.TokensToString(ts))

        Assert.AreEqual(11, ts.Count)

        Assert.AreEqual(
            "( of21.li2.co20 >< of22.li2.co21",
            ts(0).ToString)
        Assert.AreEqual(
            "number of99.li4.co30 >6< of100.li4.co31",
            ts(9).ToString)
        Assert.AreEqual(
            "string of128.li5.co20 >hello< of135.li5.co27",
            ts(10).ToString)
    End Sub

    <TestMethod()> Public Sub Tokenizer_Tokenize_and_strings_and_lines()

        Dim ts = Intrigue.Parsing.Tokenizer.Tokenize(
            Intrigue.Util.NewString(
                <string>
                    "nada"
                    "abc
cde" "volley"
                    "beach"
               </string>).Trim)

        'Console.WriteLine(Tokenizer.TokensToString(ts))

        Assert.AreEqual(4, ts.Count)

        Assert.AreEqual(
            "string of0.li1.co1 >nada< of6.li1.co7",
            ts(0).ToString)
        Assert.AreEqual(
            "string of28.li2.co21 >abc" & vbCrLf & "cde< of38.li3.co5",
            ts(1).ToString)
        Assert.AreEqual(
            "string of39.li3.co6 >volley< of47.li3.co14",
            ts(2).ToString)
        Assert.AreEqual(
            "string of69.li4.co21 >beach< of76.li4.co28",
            ts(3).ToString)
    End Sub
End Class
