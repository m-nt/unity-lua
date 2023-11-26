DebugError("hello")
obj_guid = LoadObject("TestPrefab")
DebugError(obj_guid)
while true do
    res = ObjectExists(obj_guid)
    if res then
        break
    end
end
CreateObject(obj_guid)