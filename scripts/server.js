const http = require('http');
const fs = require('fs');
const path = require('path');

const PORT = 2000;

const server = http.createServer((req, res) => {
    let filePath;
    
    if (req.url === '/') {
        filePath = path.join(__dirname, '..', 'views', 'index.html');

    } else if (req.url === '/signup'){
        filePath = path.join(__dirname, '..', 'views', 'signup.html')

    } else if (req.url === '/login') {
        filePath = path.join(__dirname, '..', 'views', 'login.html');

    } else if (req.url.startsWith('/assets/')){
        filePath = path.join(__dirname, '..', req.url);

    } else if (req.url === '/scripts/index.js'){
        filePath = path.join(__dirname, 'index.js');

    } else if (req.url === '/scripts/signUp.js'){
        filePath = path.join(__dirname, 'signUp.js');

    } else { // else que vai levar pra pagina de erro existente em views
        filePath = path.join(__dirname, '..', 'views', 'error.html');
    }
    console.log(req.url);

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
    
    fs.readFile(filePath, function(error, data) {
        if (error) {
            res.writeHead(404);
            res.end(`ERRO: ${error}`);
        } else {
            res.writeHead(200, { 'Content-Type': contentType });
            res.end(data);
        }
    });
});

server.listen(PORT, function(error) {
    if (error) {
        console.log('ERRO: ', error);
    } else {
        console.log(`Servidor ouvindo em: http://localhost:${PORT}`);
        console.log("Para encerrar servidor: Ctrl + C");
    }
});