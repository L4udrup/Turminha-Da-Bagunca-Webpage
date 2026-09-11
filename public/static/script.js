// ================================
// MENU MOBILE
// ================================

const menuToggle = document.getElementById('menuToggle');
const menu = document.getElementById('menu');

if (menuToggle && menu) {

    menuToggle.addEventListener('click', () => {

        menu.classList.toggle('open');

        if (menu.classList.contains('open')) {
            menuToggle.textContent = '✕';
        } else {
            menuToggle.textContent = '☰';
        }

    });

}


// ================================
// FECHAR MENU AO CLICAR EM UM LINK
// ================================

if (menu && menuToggle) {

    document.querySelectorAll('.menu a').forEach(link => {

        link.addEventListener('click', () => {

            menu.classList.remove('open');

            menuToggle.textContent = '☰';

        });

    });

}


// ================================
// FORMULÁRIO DE ORÇAMENTO
// ================================

const quoteForm = document.getElementById('quoteForm');
const formMessage = document.getElementById('formMessage');

if (quoteForm && formMessage) {

    quoteForm.addEventListener('submit', (event) => {

        event.preventDefault();

        const nameInput = document.getElementById('name');

        const name = nameInput
            ? nameInput.value.trim()
            : '';

        formMessage.textContent =
            `Obrigado, ${name}! Sua solicitação foi recebida. Em breve entraremos em contato.`;

        quoteForm.reset();

    });

}


// ================================
// DESTAQUE DO MENU CONFORME A SEÇÃO
// ================================

const sections =
    document.querySelectorAll('main section[id]');

const navLinks =
    document.querySelectorAll('.menu a');

if (sections.length && navLinks.length) {

    const observer = new IntersectionObserver(

        entries => {

            entries.forEach(entry => {

                if (entry.isIntersecting) {

                    navLinks.forEach(link => {

                        link.classList.remove('active');

                    });

                    const activeLink =
                        document.querySelector(
                            `.menu a[href="#${entry.target.id}"]`
                        );

                    if (activeLink) {

                        activeLink.classList.add('active');

                    }

                }

            });

        },

        {
            threshold: 0.25
        }

    );

    sections.forEach(section => {

        observer.observe(section);

    });

}


// ================================
// MOSTRAR / OCULTAR SENHA
// ================================

document.addEventListener('DOMContentLoaded', () => {

    const toggleButtons =
        document.querySelectorAll('.toggle-password');

    toggleButtons.forEach(button => {

        button.addEventListener('click', () => {

            const input = button.previousElementSibling;

            if (
                input &&
                (input.type === 'password' || input.type === 'text')
            ) {

                const isPassword =
                    input.type === 'password';

                // Alternando o tipo do input
                input.type =
                    isPassword ? 'text' : 'password';

                // Alternando o ícone
                button.textContent =
                    isPassword ? '🙈' : '👁️';

            }

        });

    });

});