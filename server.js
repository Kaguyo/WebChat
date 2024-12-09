const http = require('http');
const fs = require('fs');
const path = require('path');

const PORT = 2000;

const server = http.createServer((req, res) => {
    let filePath;
    let getIn = true;
    if (req.url === '/') {
        filePath = path.join(__dirname, 'index.html');
    } else if (req.url === '/login') {
        filePath = path.join(__dirname, 'login.html');
    } else {
        filePath = path.join(__dirname, req.url);
        getIn = false;
    }
    const extname = path.extname(filePath);

    const traduzirContentType = {
        '.html': 'text/html',
        '.css': 'text/css',
        '.js': 'application/javascript',
        '.png': 'image/png',
        '.jpg': 'image/jpeg',
        '.jpeg': 'image/jpeg',
        '.gif': 'image/gif',
        '.json': 'application/json'
    };
    
    const contentType = traduzirContentType[extname] || 'application/octet-stream';
    if (filePath != undefined){
        fs.readFile(filePath, function(error, data){
            if (error){
                res.writeHead(404);
                res.write(`ERRO: ${error}`);
            } else {
                res.writeHead(200, { 'Content-Type': contentType });
                res.end(data);
            }
        })
    } else res.end('URL sem resposta definida');
})
server.listen(PORT, function(error) {
    if (error){
        console.log('ERRO: ', error);
    } else {
        console.log(`Servidor ouvindo em: http://localhost:${PORT}`);
        console.log("Para encerrar servidor: Ctrl + C");
    }
})