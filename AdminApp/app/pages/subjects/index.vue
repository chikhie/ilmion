<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">Matières</h1>
      <!-- <UButton icon="i-heroicons-plus" color="primary" label="Ajouter" @click="isOpen = true" /> -->
    </div>

    <UCard>
      <UTable :rows="subjects" :columns="columns" :loading="pending">
        <!-- <template #actions-data="{ row }">
          <UDropdown :items="items(row)">
            <UButton color="gray" variant="ghost" icon="i-heroicons-ellipsis-horizontal-20-solid" />
          </UDropdown>
        </template> -->
      </UTable>
    </UCard>

    <!-- <UModal v-model="isOpen">
      <div class="p-4">
        <h3 class="text-lg font-bold mb-4">Ajouter une matière</h3>
        <UForm :state="state" @submit="onSubmit">
          <UFormGroup label="Nom de la matière" name="label" class="mb-4">
            <UInput v-model="state.label" />
          </UFormGroup>
          <div class="flex justify-end gap-2">
            <UButton color="gray" variant="ghost" label="Annuler" @click="isOpen = false" />
            <UButton type="submit" color="primary" label="Sauvegarder" :loading="saving" />
          </div>
        </UForm>
      </div>
    </UModal> -->
  </div>
</template>

<script setup lang="ts">
const isOpen = ref(false)
const saving = ref(false)
const state = reactive({
  label: ''
})

const columns = [
  {
    key: 'id',
    label: 'ID'
  },
  {
    key: 'label',
    label: 'Nom'
  }
]

const api = useApi()
const { data: subjects, pending, refresh } = await useAsyncData('admin-subjects', () => api.getSubjects())

// const items = (row) => [
//   [{
//     label: 'Éditer',
//     icon: 'i-heroicons-pencil-square-20-solid',
//     click: () => console.log('Edit', row.id)
//   }],
//   [{
//     label: 'Supprimer',
//     icon: 'i-heroicons-trash-20-solid',
//     click: () => deleteSubject(row.id)
//   }]
// ]

// const onSubmit = async () => {
//   saving.value = true
//   try {
//     // TODO: Implement create subject in API
//     // await api.createSubject(state)
//     isOpen.value = false
//     state.label = ''
//     refresh()
//   } catch (e) {
//     console.error(e)
//   } finally {
//     saving.value = false
//   }
// }

// const deleteSubject = async (id: number) => {
//   if (!confirm('Êtes-vous sûr ?')) return
//   try {
//     // TODO: Implement delete subject in API
//     // await api.deleteSubject(id)
//     refresh()
//   } catch (e) {
//     console.error(e)
//   }
// }
</script>
