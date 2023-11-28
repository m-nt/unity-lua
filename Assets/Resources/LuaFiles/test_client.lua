DebugError("hello")
Wait(1000)
DebugError("hello2")
obj_guid = LoadObject("TestPrefab")
DebugError(obj_guid)
for i = 0, 1000, 1 do
    if ObjectExists(obj_guid) then
        DebugError("Found the Object")
    end
end
-- while not ObjectExists(obj_guid) do
--     Wait(10)
-- end
CreateObject(obj_guid)