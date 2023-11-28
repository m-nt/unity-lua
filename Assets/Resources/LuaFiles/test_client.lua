DebugError("hello")
Wait(1000)
DebugError("hello2")
obj_guid = LoadObject("TestPrefab")
DebugError(obj_guid)
-- while not ObjectExists(obj_guid) do
--     Wait(10)
-- end
CreateObject(obj_guid)