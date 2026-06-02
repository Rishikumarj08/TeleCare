(function () {
  function setRequestInterceptor(ui) {
    try {
      const cfg = ui.getConfigs();
      cfg.requestInterceptor = function (req) {
        try {
          const token = localStorage.getItem('swagger_token');
          if (token) {
            // Add Bearer prefix automatically
            req.headers = req.headers || {};
            if (!req.headers['Authorization'] && !req.headers['authorization']) {
              req.headers['Authorization'] = 'Bearer ' + token;
            }
          }
        } catch (e) {
          // ignore
        }
        return req;
      };
    } catch (e) {
      // ignore
    }
  }

  function addTokenUi() {
    var topbar = document.querySelector('.swagger-ui .topbar');
    if (!topbar) return;
    // Avoid adding multiple buttons
    if (document.getElementById('set-swagger-token-btn')) return;

    var btn = document.createElement('button');
    btn.id = 'set-swagger-token-btn';
    btn.innerText = 'Set Token';
    btn.style.marginLeft = '12px';
    btn.style.padding = '6px 10px';
    btn.style.borderRadius = '4px';
    btn.style.border = '1px solid #ccc';
    btn.style.background = '#f5f5f5';
    btn.onclick = function () {
      var t = prompt('Paste JWT token (without the "Bearer " prefix):');
      if (t !== null) {
        localStorage.setItem('swagger_token', t);
        alert('Token saved to session. Click Authorize or execute an endpoint to use it.');
      }
    };
    topbar.appendChild(btn);
  }

  window.addEventListener('load', function () {
    var tryInit = function () {
      if (window.ui && window.ui.getConfigs) {
        setRequestInterceptor(window.ui);
        addTokenUi();
      } else {
        setTimeout(tryInit, 300);
      }
    };
    tryInit();
  });
})();
