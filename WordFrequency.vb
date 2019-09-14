' Project name: WordFrequency
' Description: VBA macro that calculates word frequency
' Written by: Chris Tzatzakis
' Memo: Use it freely and support the open source community

Sub WordFrequency()
    Const maxwords = 100000        'Maximum unique words allowed - assign based on requirements
    Dim SingleWord As String       'Raw word pulled from doc
    Dim Words(maxwords) As String  'Array to hold unique words
    Dim Freq(maxwords) As Integer  'Frequency counter for unique words
    Dim WordNum As Integer         'Number of unique words
    Dim ttlwds As Long             'Total words in the document
    Dim Excludes As String         'Words to be excluded
    Dim Found As Boolean           'Temporary flag
    Dim j, k, l, Temp As Integer   'Temporary variables
    Dim a_word As Object           '
    Dim tmpName As String          '

    ' Set up excluded words
    Excludes = "[the][a][of][is][to][for][by][be][and][are][in][at][on]"
   
    Selection.HomeKey Unit:=wdStory
    System.Cursor = wdCursorWait
    WordNum = 0
    ttlwds = ActiveDocument.Words.Count

    ' Manage the repeat
    For Each a_word In ActiveDocument.Words
        SingleWord = Trim(LCase(a_word))
        'Out of range?
        If SingleWord < "a" Or SingleWord > "z" Then
            SingleWord = ""
        End If
        'On exclude list?
        If InStr(Excludes, "[" & SingleWord & "]") Then
            SingleWord = ""
        End If
        If Len(SingleWord) > 0 Then
            Found = False
            For j = 1 To WordNum
                If Words(j) = SingleWord Then
                    Freq(j) = Freq(j) + 1
                    Found = True
                    Exit For
                End If
            Next j
            If Not Found Then
                WordNum = WordNum + 1
                Words(WordNum) = SingleWord
                Freq(WordNum) = 1
            End If
            If WordNum > maxwords - 1 Then
                j = MsgBox("Too many words.", vbOKOnly)
                Exit For
            End If
        End If
        ttlwds = ttlwds - 1
        StatusBar = "Remaining: " & ttlwds & ", Unique: " & WordNum
    Next a_word

    ' Results
    tmpName = ActiveDocument.AttachedTemplate.FullName
    Documents.Add Template:=tmpName, NewTemplate:=False
    Selection.ParagraphFormat.TabStops.ClearAll
    With Selection
        For j = 1 To WordNum
            .TypeText Text:=Trim(Str(Freq(j))) _
              & vbTab & Words(j) & vbCrLf
        Next j
    End With
    System.Cursor = wdCursorNormal
    j = MsgBox("There were " & Trim(Str(WordNum)) & _
      " different words ", vbOKOnly, "Finished")
End Sub