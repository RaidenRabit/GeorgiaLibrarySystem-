@ECHO OFF

SET SQLCMD="C:\Program Files\Microsoft SQL Server\110\Tools\Binn\SQLCMD.EXE"

SET SERVER="localhost"

SET DB="master"

SET LOGIN="SA"

SET PASSWORD="12345"

SET INPUT=%cd%\SQLCreateQuery.sql

%SQLCMD% -S%SERVER% -d%DB% -U%LOGIN% -P%PASSWORD% -i%INPUT% -b
