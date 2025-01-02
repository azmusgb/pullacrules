document.getElementById('processButton').addEventListener('click', async () => {
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = '.fwd';
    fileInput.onchange = async () => {
        const file = fileInput.files[0];
        if (!file) return;

        const formData = new FormData();
        formData.append('file', file);

        document.getElementById('status').innerText = 'Status: Uploading and Processing...';

        try {
            const response = await fetch('https://your-api-url/api/processing/process', {
                method: 'POST',
                body: formData,
            });

            const result = await response.json();
            if (response.ok) {
                document.getElementById('status').innerText = `Status: ${result.message}`;
            } else {
                document.getElementById('status').innerText = `Error: ${result.error}`;
            }
        } catch (error) {
            console.error('Processing failed:', error);
            document.getElementById('status').innerText = 'Error: Processing failed.';
        }
    };

    fileInput.click();
});

// Register Service Worker
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('service-worker.js')
        .then(() => console.log('Service Worker Registered'))
        .catch(err => console.error('Service Worker Registration Failed:', err));
}
