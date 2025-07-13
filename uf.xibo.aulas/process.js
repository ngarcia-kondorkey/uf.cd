        
        // Configuration
        const pageSize = 10; // Number of items per page
		let currentPage = 1;
        let totalPages = 1; // Will be updated after the first API call
		let idCartelera = 1;
		let estimado = 0;
		let fetchInterval = 0;//5 * 1000;
        let autoAdvanceInterval = null;
				
        // Function to fetch and display data
        async function fetchData(page) {
			
			// DOM Elements
			const coursesData = document.getElementById('courses-data');
			const paginationInfo = document.getElementById('pagination-info');
			const loadingIndicator = document.getElementById('loading-indicator');
			const errorMessage = document.getElementById('error-message');
			
            loadingIndicator.classList.remove('hidden');
			if (errorMessage) {
				errorMessage.classList.add('hidden');
			}
            coursesData.innerHTML = ''; // Clear previous data

            try {
                const response = await fetch(`http://localhost:8080/api/CarteleraDigital/GetReporteCarteleraDigital?idCartelera=${idCartelera}&estimado=${estimado}&pageNumber=${page}&pageSize=${pageSize}`);
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const data = await response.json();
                const items = data.items;

                // Update pagination state
                currentPage = data.pageNumber;
                totalPages = data.totalPages;

                // Render table rows
				const columnNumber = 7;
                if (items.length === 0) {
                    dataTableBody.innerHTML = `<tr><td colspan="${columnNumber}" class="text-center py-4 text-gray-500">No data found.</td></tr>`;
                } else {
					let careerName = '';
					let careerId = 0;
					let dataTableBody = null;
					for (let i = 0; i < items.length; i++) {
						const currentItem = items[i];
						if (careerName != currentItem.carrera) { // Check if it's the same career
							if (careerName != '') { // If it's not the first career
								
							}
							careerName = currentItem.carrera;
							careerId++;
							
							const h1CareerName = document.createElement('h1');
							h1CareerName.innerHTML = careerName;
							h1CareerName.classList.add("career-title");
							h1CareerName.classList.add("mt-5");
							h1CareerName.classList.add("mb-1");
							h1CareerName.classList.add("ms-1");
							coursesData.appendChild(h1CareerName);
							
							const divTable = document.createElement('div');
							divTable.innerHTML = getCoursesTableTemplate(careerId);
							coursesData.appendChild(divTable);
							
							dataTableBody = document.getElementById(`data-table-body-${careerId}`);
						}
						
						const row = document.createElement('tr');
						row.innerHTML = `
							<td class="py-2 px-2">${currentItem.actividad}</td>
							<td class="py-2 px-2">${currentItem.id_comision}</td>
							<td class="py-2 px-2">${currentItem.horario_ini} - ${currentItem.horario_fin}</td>
							<td class="py-2 px-2">${currentItem.aula}</td>
							<td class="py-2 px-2">${currentItem.ubicacion}</td>
							<td class="py-2 px-2">${currentItem.detalle_cartelera}</td>
							<td class="py-2 px-2">${currentItem.mat_doc}</td>
						`;
						dataTableBody.appendChild(row);
					}
                }

                // Update pagination info
				if (paginationInfo) {
					paginationInfo.textContent = `Página ${currentPage} de ${totalPages}`;
				}
                

            } catch (error) {
                console.error('Error fetching data:', error);
				if (errorMessage) {
					errorMessage.textContent = `Failed to load data: ${error.message}`;
					errorMessage.classList.remove('hidden');
				}
                dataTableBody.innerHTML = `<tr><td colspan="4" class="text-center py-4 text-red-500">Error loading data. Please try again.</td></tr>`;
            } finally {
                loadingIndicator.classList.add('hidden');
            }
        }
		
		function getCoursesTableTemplate(id) {
			return `
				<!--<div class="overflow-x-auto shadow-md">-->
				<div class="overflow-x-auto rounded-lg shadow-md">
					<table class="min-w-full bg-white">
						<thead>
							<tr>
								<th class="py-2 px-2 border-gray-200 tblcol-1">NOMBRE DEL CURSO O ACTIVIDAD</th>
								<th class="py-2 px-2 border-gray-200 tblcol-2">COMISIÓN</th>
								<th class="py-2 px-2 border-gray-200 tblcol-3">HORARIO</th>
								<th class="py-2 px-2 border-gray-200 tblcol-4">AULA</th>
								<th class="py-2 px-2 border-gray-200 tblcol-5">UBICACIÓN</th>
								<th class="py-2 px-2 border-gray-200 tblcol-6">DETALLE</th>
								<th class="py-2 px-2 border-gray-200 tblcol-7">DOCENTE / RESPONSABLE</th>
							</tr>
						</thead>
						<tbody id="data-table-body-${id}">
						</tbody>
					</table>
				</div>
			`;
		}

        // Function to advance to the next page automatically
        function autoAdvancePage() {
            if (currentPage < totalPages) {
                currentPage++;
            } else {
                currentPage = 1; // Loop back to the first page
            }
            fetchData(currentPage);
        }

        // Initial fetch when the page loads
        document.addEventListener('DOMContentLoaded', () => {
            fetchData(currentPage);
			
			if (fetchInterval > 0) { // Check if timer is disabled (fetchInterval <= 0)
				autoAdvanceInterval = setInterval(autoAdvancePage, fetchInterval); // Start automatic page advancement
			}
        });

        // Clear interval when the page is closed or navigated away from
        window.addEventListener('beforeunload', () => {
            clearInterval(autoAdvanceInterval);
        });