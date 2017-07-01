console.log("Hello, World!");

var http = require('http');

http.createServer(function (req, res) {
    var html = buildHtml(req);

    res.writeHead(200, {
        'Content-Type': 'text/html',
        'Content-Length': html.length,
        'Expires': new Date().toUTCString()
    });

    res.end(html);
}).listen(80);

function buildHtml(req) {
    var title = 'Hello World!';
    var body = "The date and time are currently: " + Date();

    return '<!DOCTYPE html>'
        + '<html><head><title>' + title + '</title></head><body>' + body + '</body></html>';
};
