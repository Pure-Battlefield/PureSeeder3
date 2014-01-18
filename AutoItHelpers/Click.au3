;Click.au3
#NoTrayIcon
#include <Constants.au3>
#include <Misc.au3>

If $CmdLine[0] <> 1 Then
	ConsoleWrite("No command line arg.")
	Exit(1)
EndIf

$ProcessName = $CmdLine[1]

$Full = WinGetTitle ($ProcessName) ; Get The Full Title..
$HWnD = WinGetHandle ($Full) ; Get The Handle
;$Full = _GetWinTitleFromProcName($ProcessName)
;$HWnD = _GetHandleFromProcName($ProcessName)
$iButton = 'Left' ; Button The Mouse Will Click I.E. "Left Or Right"
$iClicks = '1' ; The Number Of Times To Click
$iX = '30' ; The "X" Pos For The Mouse To Click
$iY = '30' ; The "Y" Pos For The Mouse To Click
If IsHWnD ($HWnD) And WinExists ($Full) <> '0' Then ; Win Check
	ConsoleWrite("WinExists")
	ControlClick ($HWnD, '','', $iButton, $iClicks, $iX, $iY) ; Clicking The Window While Its Minmized
	Exit(0)
EndIf
ConsoleWrite("Win doesn't exist")
Exit(2)

Func _GetHandleFromProcName($s_procName)
	$pid = ProcessExists($s_ProcName)
    $a_list = WinList()
    For $i = 1 To $a_list[0][0]
        If $a_list[$i][0] <> "" Then
            $pid2 = WinGetProcess($a_list[$i][0])
            If $pid = $pid2 Then Return $a_list[$i][1]
        EndIf
    Next
EndFunc

Func _GetWinTitleFromProcName($s_ProcName)
    $pid = ProcessExists($s_ProcName)
    $a_list = WinList()
    For $i = 1 To $a_list[0][0]
        If $a_list[$i][0] <> "" Then
            $pid2 = WinGetProcess($a_list[$i][0])
            If $pid = $pid2 Then Return $a_list[$i][0]
        EndIf
    Next
EndFunc   ;==>_GetWinTitleFromProcName