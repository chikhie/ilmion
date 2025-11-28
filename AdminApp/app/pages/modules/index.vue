<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">Modules</h1>
      <UButton icon="i-heroicons-plus" color="primary" label="Ajouter" @click="isOpen = true" />
    </div>

    <UCard>
      <div class="flex gap-4 mb-4">
        <USelect v-model="filterSubject" :options="subjectOptions" placeholder="Filtrer par matière" />
      </div>

      <UTable :rows="filteredModules" :columns="columns" :loading="pending">
        <template #actions-data="{ row }">
          <UDropdown :items="items(row)">
            <UButton color="gray" variant="ghost" icon="i-heroicons-ellipsis-horizontal-20-solid" />
          </UDropdown>
        </template>
      </UTable>
    </UCard>

    <UModal v-model="isOpen">
      <div class="p-4">
        <h3 class="text-lg font-bold mb-4">Ajouter un module</h3>
        <UForm :state="state" @submit="onSubmit">
          <UFormGroup label="Matière" name="subjectId" class="mb-4" required>
            <USelect v-model="state.subjectId" :options="subjectOptions" option-attribute="label" value-attribute="value" />
          </UFormGroup>
          
          <UFormGroup label="Nom du module" name="label" class="mb-4" required>
            <UInput v-model="state.label" />
          </UFormGroup>
          
          <div class="flex justify-end gap-2">
            <UButton color="gray" variant="ghost" label="Annuler" @click="isOpen = false" />
            <UButton type="submit" color="primary" label="Sauvegarder" :loading="saving" />
          </div>
        </UForm>
      </div>
    </UModal>
  </div>
</template>

<script setup lang="ts">
const isOpen = ref(false)
const saving = ref(false)
const filterSubject = ref('')
const state = reactive({
  label: '',
  subjectId: ''
})

const columns = [
  { key: 'label', label: 'Nom' },
  { key: 'subjectName', label: 'Matière' },
  { key: 'chapterCount', label: 'Chapitres' },
  // { key: 'displayOrder', label: 'Ordre' },
  { key: 'actions' }
]

const api = useApi()

// Charger les matières pour le filtre
const { data: subjects } = await useAsyncData('admin-subjects', () => api.getSubjects())

const subjectOptions = computed(() => subjects.value?.map(s => ({ label: s.label, value: s.id })) || [])

// Charger les modules (on charge tout ou on filtre selon la sélection)
// L'API backend filtre par subjectId. Si rien n'est sélectionné, on ne charge rien ou on charge tout si possible ?
// L'endpoint est /api/Module/subject/{subjectId}. Il n'y a pas de endpoint "get all modules".
// Donc on doit attendre qu'une matière soit sélectionnée, ou alors on boucle sur toutes les matières (pas optimal mais ok pour admin si peu de matières).

// Pour l'instant, on force la sélection d'une matière pour voir les modules
watch(subjects, (newSubjects) => {
  if (newSubjects && newSubjects.length > 0 && !filterSubject.value) {
    // Sélectionner la première matière par défaut
    // filterSubject.value = newSubjects[0].id.toString()
  }
})

const { data: modules, pending, refresh } = await useAsyncData(
  'admin-modules',
  async () => {
    if (!filterSubject.value) return []
    return await api.getModulesBySubject(parseInt(filterSubject.value))
  },
  {
    watch: [filterSubject]
  }
)

const filteredModules = computed(() => modules.value || [])

const items = (row) => [
  [{
    label: 'Voir Chapitres',
    icon: 'i-heroicons-document-text-20-solid',
    click: () => navigateTo(`/chapters?moduleId=${row.id}`)
  }],
  [{
    label: 'Éditer',
    icon: 'i-heroicons-pencil-square-20-solid',
    click: () => console.log('Edit', row.id)
  }],
  [{
    label: 'Supprimer',
    icon: 'i-heroicons-trash-20-solid',
    click: () => deleteModule(row.id)
  }]
]

const onSubmit = async () => {
  if (!state.subjectId || !state.label) return
  
  saving.value = true
  try {
    await api.createModule({
      label: state.label,
      subjectId: parseInt(state.subjectId)
    })
    isOpen.value = false
    state.label = ''
    refresh()
  } catch (e) {
    console.error(e)
    alert('Erreur lors de la création')
  } finally {
    saving.value = false
  }
}

const deleteModule = async (id: string) => {
  if (!confirm('Êtes-vous sûr ?')) return
  try {
    await api.deleteModule(id)
    refresh()
  } catch (e) {
    console.error(e)
  }
}
</script>
