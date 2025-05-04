document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150;

    
    document.querySelectorAll('[data-modal="true"]').forEach(button => {
        button.addEventListener('click', () => {
            const modal = document.querySelector(button.getAttribute('data-target'));
            if (modal) modal.style.display = 'flex';
        });
    });

    
    document.querySelectorAll('[data-close="true"]').forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal');
            if (modal) {
                modal.style.display = 'none';
                modal.querySelectorAll('form').forEach(form => {
                    form.reset();
                    const imagePreview = form.querySelector('.image-preview');
                    if (imagePreview) imagePreview.src = '';
                    const imagePreviewer = form.querySelector('.image-previewer');
                    if (imagePreviewer) imagePreviewer.classList.remove('selected');
                });
            }
        });
    });

    
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]');
        const imagePreview = previewer.querySelector('.image-preview');
        previewer.addEventListener('click', () => fileInput.click());

        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0];
            if (file) processImage(file, imagePreview, previewer, previewSize);
        });
    });

    
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            clearErrorMessages(form);
            const formData = new FormData(form);

            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                });

                if (res.ok) {
                    const modal = form.closest('.modal');
                    if (modal) modal.style.display = 'none';
                    window.location.reload();
                } else if (res.status === 400) {
                    const data = await res.json();
                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            const input = form.querySelector(`[name="${key}"]`);
                            if (input) input.classList.add('input-validation-error');
                            const span = form.querySelector(`[data-valmsg-for="${key}"]`);
                            if (span) {
                                span.innerText = data.errors[key].join('\n');
                                span.classList.add('field-validation-error');
                            }
                        });
                    }
                }
            } catch {
                console.log('Error submitting the form');
            }
        });
    });

    
    document.querySelectorAll('.project-options-toggle').forEach(toggle => {
        toggle.addEventListener('click', (e) => {
            e.stopPropagation();
            const parent = toggle.closest('.project-options');
            parent.classList.toggle('show');

            document.querySelectorAll('.project-options').forEach(opt => {
                if (opt !== parent) opt.classList.remove('show');
            });
        });
    });

    
    document.addEventListener('click', () => {
        document.querySelectorAll('.project-options').forEach(opt => {
            opt.classList.remove('show');
        });
    });

    
    document.querySelectorAll('.project-option[data-action="edit"]').forEach(button => {
        button.addEventListener('click', () => {
            const card = button.closest('.project-cards');
            const modal = document.querySelector('#editProjectModal');
            if (!card || !modal) return;

            modal.querySelector('[name="Id"]').value = card.dataset.id || '';
            modal.querySelector('[name="Id"]').value = card.dataset.id || '';
            modal.querySelector('[name="ProjectName"]').value = card.dataset.name || '';
            modal.querySelector('[name="ClientName"]').value = card.dataset.client || '';
            modal.querySelector('[name="Description"]').value = card.dataset.description || '';
            modal.querySelector('[name="StartDate"]').value = card.dataset.startdate || '';
            modal.querySelector('[name="EndDate"]').value = card.dataset.enddate || '';
            modal.querySelector('[name="Budget"]').value = card.dataset.budget || '';

            modal.style.display = 'flex';
        });
    });

    
    document.querySelectorAll('.project-option[data-action="delete"]').forEach(button => {
        button.addEventListener('click', async () => {
            const id = button.getAttribute('data-id');
            if (!id) return;

            const confirmDelete = confirm("Are you sure you want to delete this project?");
            if (!confirmDelete) return;

            try {
                const res = await fetch(`/projects/delete`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body: `id=${encodeURIComponent(id)}`
                });

                if (res.ok) {
                    window.location.reload();
                } else {
                    const error = await res.text();
                    alert("Error deleting project:\n" + error);
                }
            } catch (error) {
                alert("Failed to connect to server.");
            }
        });
    });
});


function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error');
    });
    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = '';
        span.classList.remove('field.validation-error');
    });
}

function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onerror = () => reject(new Error("Failed to load file."));
        reader.onload = (e) => {
            const img = new Image();
            img.onerror = () => reject(new Error("Failed to load image."));
            img.onload = () => resolve(img);
            img.src = e.target.result;
        };
        reader.readAsDataURL(file);
    });
}

async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file);
        const canvas = document.createElement('canvas');
        canvas.width = previewSize;
        canvas.height = previewSize;
        const ctx = canvas.getContext('2d');
        ctx.drawImage(img, 0, 0, previewSize, previewSize);
        imagePreview.src = canvas.toDataURL('image/jpeg');
        previewer.classList.add('selected');
    } catch (error) {
        console.error('Failed on image-processing:', error);
    }
}

async function toggleProfileMenu() {
    const menu = document.querySelector('.profile-menu');
    menu.classList.toggle('hidden');
}

async function logout() {
    const form = document.getElementById('logoutForm');
    if (form) {
        form.submit();
    }
}

