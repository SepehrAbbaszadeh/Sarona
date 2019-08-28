use Sarona
Go

update Sarona.dbo.NumberingPools
set NormalizedSubscriberName = REPLACE(SubscriberName,' ','') 