const http = require('http');
const PORT = 25500;

let messages = [];

// Cria o servidor HTTP
const server = http.createServer((req, res) => {
    // Configura os cabeçalhos para permitir acesso externo
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Content-Type', 'application/json');

    // Rota para POST - Receber nova mensagem
    if (req.method === 'POST' && req.url === '/send') {
        let body = '';

        // Recebe os dados da requisição
        req.on('data', chunk => {
            body += chunk.toString();
        });

        req.on('end', () => {
            const message = JSON.parse(body); // Converte o JSON recebido
            messages.push(message); // Adiciona a nova mensagem ao array
            res.end(JSON.stringify({ status: 'Message received' }));
        });

    // Rota para GET - Listar todas as mensagens
    } else if (req.method === 'GET' && req.url === '/messages') {
        res.end(JSON.stringify(messages));

    // Rota padrão para requisições inválidas
    } else {
        res.statusCode = 404;
        res.end(JSON.stringify({ error: 'Route not found' }));
    }
});

// Inicia o servidor
server.listen(PORT, '127.0.0.1', () => {
    console.log(`Servidor rodando em http://127.0.0.1:${PORT}`);
});