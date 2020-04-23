
var funstr = " int test(char*p1, char*p2, int a) ";

var regexp1 = /(.*) {1,}(.*) {0,}\((.*)\) {0,}/
var res = regexp1.exec(funstr);
console.info(res);

var params = res[3].trim();
console.info(params);

var arr = params.split(",");
for(var i = 0; i<arr.length; i++)
{
    console.info(arr[i].trim());
    var param = arr[i].trim();
    if (param.indexOf("*") >= 0)
    {
        console.info("#$ " + param);
    }
    else if(param.indexOf("&") >= 0)
    {
        console.info("#2 " + param);
    }
    else
    {
        
    }
}





