DebugError("hello")
obj_guid = LoadObject("TestPrefab")
DebugError(obj_guid)
while not ObjectExists(obj_guid) do
    Wait(10)
end
RegisterCommand("hello", function (sender, args)
    DebugError("hello from lua")
end)
CreateObject(obj_guid)
-- TriggerCommand("hello")
